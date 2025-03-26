using CRM.Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Presentation.Components {
    public class UsersTable : ViewComponent {

        public UsersTable(UserManager<User> userManager) {
            _userManager = userManager;
        }

        private readonly UserManager<User> _userManager;

        public async Task<IViewComponentResult> InvokeAsync() {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
    }
}
