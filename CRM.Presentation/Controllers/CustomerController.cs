using System.Net;
using CRM.Business.Abstract;
using CRM.Core.Extensions;
using CRM.Entity.Concrete;
using CRM.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Presentation.Controllers {
    public class CustomerController : Controller {

        public CustomerController(ICustomerBusiness customerBusiness, IDbLogger dbLogger) {
            _customerBusiness = customerBusiness;
            _dbLogger = dbLogger;
        }

        private readonly ICustomerBusiness _customerBusiness;
        private readonly IDbLogger _dbLogger;

        public IActionResult Add(CustomerTableViewModel customerViewModel) {
            try {
                if(customerViewModel.IsCreated || customerViewModel.Email == null) {
                    ModelState.Clear();
                }
                return View(customerViewModel);
            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage($"{customerViewModel} oluşturulduktan sonra hata", LogLevel.Error, ex);
            }
            return View();
        }

        public async Task<IActionResult> Form(CustomerTableViewModel customerViewModel) {
            try {
                ModelState.Remove("Regions");
                ModelState.Remove("Customers");

                if(ModelState.IsValid) {
                    if(customerViewModel.Id == 0) {
                        var customer = new Customer {
                            FirstName = customerViewModel.FirstName,
                            LastName = customerViewModel.LastName,
                            Email = customerViewModel.Email,
                            Region = customerViewModel.Region
                        };

                        try {
                            var addedCustomer = await _customerBusiness.AddAsync(customer);
                            if(addedCustomer != null) {
                                TempData["customer"] = $"{customer.FirstName} {customer.LastName}";
                                _dbLogger.LogToDbByMessage($"{customer} müşterisi oluşturuldu", LogLevel.Information);
                                return RedirectToAction(nameof(Add), new CustomerTableViewModel() {
                                    IsCreated = true
                                });
                            }
                        }
                        catch(DbUpdateException dbEx) {
                            var exMsg = dbEx.InnerException?.Message ?? dbEx.Message;
                            if(exMsg.Contains("duplicate")) {
                                _dbLogger.LogToDbByMessage("Müşteri eklenirken hata", LogLevel.Error, dbEx);
                                customerViewModel.Errors.Add($"{customerViewModel.Email} email is already exist!");
                            }
                            else {
                                customerViewModel.Errors.Add(exMsg);
                            }
                            return View(nameof(Add), customerViewModel);
                        }
                    }
                    else {
                        var customerToUpdate = await _customerBusiness.FirstOrDefaultAsync(c => c.Id == customerViewModel.Id);
                        if(customerToUpdate != null) {
                            customerToUpdate.FirstName = customerViewModel.FirstName;
                            customerToUpdate.LastName = customerViewModel.LastName;
                            customerToUpdate.Email = customerViewModel.Email;
                            customerToUpdate.RegistrationDate = customerViewModel.RegistrationDate;
                            customerToUpdate.Region = customerViewModel.Region?.ToUpperFirstLetter();
                        }
                        try {
                            var updatedCustomer = await _customerBusiness.UpdateAsync(customerToUpdate!);
                            _dbLogger.LogToDbByMessage($"{customerToUpdate} müşterisi güncellendi", LogLevel.Information);
                            return Json(updatedCustomer);
                        }
                        catch(DbUpdateException dbEx) {
                            var exMsg = dbEx.InnerException?.Message ?? dbEx.Message;
                            if(exMsg.Contains("duplicate")) {
                                _dbLogger.LogToDbByMessage($"{customerToUpdate} müşterisi güncellenirken hata", LogLevel.Error, dbEx);
                                customerViewModel.Errors.Add($"{customerViewModel.Email} email is already exist!");
                            }
                            else {
                                customerViewModel.Errors.Add(exMsg);
                            }
                            return new JsonResult(customerToUpdate) { StatusCode = (int)HttpStatusCode.BadRequest };
                        }
                    }
                }
                else {
                    return View(nameof(Index), customerViewModel);
                }

            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage("Müşteri formunda hata", LogLevel.Error, ex);
            }
            return BadRequest();
        }

        public IActionResult? Update([FromQuery] CustomerTableViewModel customerViewModel) {
            ModelState.Remove("Regions");
            ModelState.Remove("Customers");
            if(ModelState.IsValid) {
                return RedirectToAction(nameof(Form), new {
                    customerViewModel.Id,
                    customerViewModel.FirstName,
                    customerViewModel.LastName,
                    customerViewModel.Email,
                    customerViewModel.Region,
                    customerViewModel.RegistrationDate
                });
            }
            return default;
        }

        public async Task<IActionResult> Delete(string id) {
            try {
                var deletedCustomer = await _customerBusiness.DeleteByIdAsync(id);
                if(deletedCustomer != null) {
                    _dbLogger.LogToDbByMessage($"{deletedCustomer} müşterisi silindi", LogLevel.Information);
                }
                return Json(deletedCustomer);
            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage("Müşteri silinrken hata", LogLevel.Error, ex);
            }
            return BadRequest();
        }
    }
}
