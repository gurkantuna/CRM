using CRM.Core.Managers.Concrete;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;
using CRM.Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRM.Business.Managers {
    public class CrmDbControlManager : DbControlManager<CrmDbContext> {
        public CrmDbControlManager(CrmDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager) : base(context) {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public async override Task WriteMockDataAfterCreate() {
            if(!await Context.Customers.AnyAsync()) {
                Context.Customers.AddRange(
                    new Customer {
                        Email = "john.doe@example.com",
                        FirstName = "John",
                        LastName = "Doe",
                        Region = "North America",
                        RegistrationDate = new DateTime(2023, 06, 15)
                    },
                    new Customer {
                        Email = "jane.smith@example.com",
                        FirstName = "Jane",
                        LastName = "Smith",
                        Region = "Europe",
                        RegistrationDate = new DateTime(2023, 05, 10)
                    },
                    new Customer {
                        Email = "carlos.gomez@example.com",
                        FirstName = "Carlos",
                        LastName = "Gomez",
                        Region = "South America",
                        RegistrationDate = new DateTime(2023, 07, 22)
                    }
                );
            }

            if(!await Context.Users.AnyAsync()) {

                var adminUser = new User {
                    Email = "admin@admin.com",
                    UserName = "admin",
                    PasswordHash = "123"
                };
                var result = await _userManager.CreateAsync(adminUser, adminUser.PasswordHash);

                var role = "Admin";
                if(!await _roleManager.RoleExistsAsync(role)) {
                    result = await _roleManager.CreateAsync(new IdentityRole<int>(role));
                }

                if(result.Succeeded) {
                    var resultRole = await _userManager.AddToRoleAsync(adminUser, role);
                    if(resultRole.Succeeded) {
                        Console.WriteLine("admin kullanıcısı 123 şifresi ve Admin yetkisi ile eklendi");
                    }
                }

                var affectedRows = await Context.SaveChangesAsync();
                if(affectedRows > 0) {
                    Console.WriteLine("Yeni müşteriler başarıyla eklendi");
                }
            }
        }
    }
}