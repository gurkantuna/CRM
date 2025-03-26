using CRM.Business.Abstract;
using CRM.Entity.Concrete;
using CRM.Presentation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Presentation.Components {
    public class DashboardHeader : ViewComponent {

        public DashboardHeader(ICustomerBusiness customerBusiness,
            UserManager<User> userManager) {
            _customerBusiness = customerBusiness;
            _userManager = userManager;
        }

        private readonly ICustomerBusiness _customerBusiness;
        private readonly UserManager<User> _userManager;

        public async Task<IViewComponentResult> InvokeAsync() {
            var customerCount = await _customerBusiness.CountAsync();

            var dashboardHeaderViewModel = new DashboardHeaderViewModel {
                CustomerCount = customerCount,
                UserCount = _userManager.Users.Count()
            };
            return View(dashboardHeaderViewModel);
        }
    }
}