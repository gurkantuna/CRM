using Microsoft.AspNetCore.Mvc;

namespace CRM.Presentation.Controllers {
    public class UserController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
