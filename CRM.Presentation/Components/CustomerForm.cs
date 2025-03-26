using CRM.Business.Abstract;
using CRM.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRM.Presentation.Components {
    public class CustomerForm : ViewComponent {
        public CustomerForm(IRegionBusiness regionBusiness) {
            _regionBusiness = regionBusiness;
        }

        private readonly IRegionBusiness _regionBusiness;

        public async Task<IViewComponentResult> InvokeAsync() {
            var regions = await _regionBusiness.GetAsync();
            var customerModel = new CustomerTableViewModel {
                Regions = new SelectList(regions, "Name", "Name", "Turkey")
            };
            return View(customerModel);
        }
    }
}