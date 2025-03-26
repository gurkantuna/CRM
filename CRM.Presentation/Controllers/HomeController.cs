using System.Diagnostics;
using CRM.Core.Infrastructure.Concrete.DotNetCore;
using CRM.Core.Managers.Abstract;
using CRM.Core.ViewModels.Concrete;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Presentation.Controllers {
    [Authorize]
    public class HomeController : DbCheckStatusController<CrmDbContext> {

        public HomeController(IDbControlManager<CrmDbContext> dbControlManager) : base(dbControlManager) { }

        public IActionResult Index() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
