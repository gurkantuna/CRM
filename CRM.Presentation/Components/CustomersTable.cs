using CRM.Business.Abstract;
using CRM.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRM.Presentation.Components {
    public class CustomersTable : ViewComponent {

        public CustomersTable(ICustomerBusiness customerBusiness, IRegionBusiness regionBusiness) {
            _customerBusiness = customerBusiness;
            _regionBusiness = regionBusiness;
        }

        private readonly ICustomerBusiness _customerBusiness;
        private readonly IRegionBusiness _regionBusiness;

        public async Task<IViewComponentResult> InvokeAsync() {
            var customers = await _customerBusiness.GetAsync();
            var regions = await _regionBusiness.GetAsync();

            var customerTableViewModel = new CustomerTableViewModel {
                Customers = customers,
                Regions = new SelectList(regions, "Name", "Name", "Turkey")
            };
            return View(customerTableViewModel);
        }
    }
}