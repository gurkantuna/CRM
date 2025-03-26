using System.Net;
using CRM.API.Model;
using CRM.Business.Abstract;
using CRM.Business.Utilities.IoC.DotNetCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRM.API.Controllers {
    [ApiController]
    [Route("[controller]" + "s")]
    public class CustomerController : ControllerBase {

        public CustomerController(ICustomerBusiness customerBusiness, IDbLogger dbLogger) {
            _customerBusiness = BusinessServiceTool.ServiceProvider.GetRequiredService<ICustomerBusiness>();
            _dbLogger = dbLogger;
        }

        private readonly ICustomerBusiness _customerBusiness;
        private readonly IDbLogger _dbLogger;

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int? id) {
            try {
                if(id != null) {
                    var existCustomer = await _customerBusiness.GetByIdAsync(id);
                    if(existCustomer != null) {
                        return new JsonResult(existCustomer);
                    }
                    else {
                        return new JsonResult(new { }) { StatusCode = (int)HttpStatusCode.RequestedRangeNotSatisfiable };
                    }
                }
                else {
                    var customers = await _customerBusiness.GetAsync();
                    return new JsonResult(customers);
                }

            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage($"{id} idli müþteri silinirken hata", LogLevel.Error, ex);
            }
            return new JsonResult(default) { StatusCode = (int)HttpStatusCode.BadRequest };
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CustomerUpdateModel customerUpdateModel) {
            try {
                /*
                 Örnek bir update object
                  
                 Content-Type : multipart/form-data;

                 {
                 "Id" : 3,
                 "FirstName" : "john"
                 "LastName" : "doe"
                 "Email" : "john@doe.com"
                 "Region" : "Asia"
                 }
                 */
                if(customerUpdateModel != null) {
                    //throw new Exception("deneme");
                    var existCustomer = await _customerBusiness.GetByIdAsync(customerUpdateModel.Id);
                    if(existCustomer != null) {
                        existCustomer.FirstName = customerUpdateModel.FirstName;
                        existCustomer.LastName = customerUpdateModel.LastName;
                        existCustomer.Email = customerUpdateModel.Email;
                        existCustomer.Region = customerUpdateModel.Region;
                        var updatedCustomer = await _customerBusiness.UpdateAsync(existCustomer);
                        return new JsonResult(updatedCustomer);
                    }
                    else {
                        return new JsonResult(new { }) { StatusCode = (int)HttpStatusCode.RequestedRangeNotSatisfiable };
                    }
                }
                else {
                    return new JsonResult(new { }) { StatusCode = (int)HttpStatusCode.RequestedRangeNotSatisfiable };
                }
            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage($"{customerUpdateModel.Id} idli müþteri güncellenirken hata- json data: {JsonConvert.SerializeObject(customerUpdateModel)}",
                                            LogLevel.Error,
                                            ex);
            }
            return new JsonResult(default) { StatusCode = (int)HttpStatusCode.BadRequest };
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) {
            try {
                var deletedCustomer = await _customerBusiness.DeleteByIdAsync(id);
                if(deletedCustomer != null) {
                    return new JsonResult(deletedCustomer);
                }
                else {
                    return new JsonResult(new { }) { StatusCode = (int)HttpStatusCode.RequestedRangeNotSatisfiable };
                }
            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage($"{id} idli müþteri silinirken hata", LogLevel.Error, ex);
            }
            return new JsonResult(default) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
    }
}
