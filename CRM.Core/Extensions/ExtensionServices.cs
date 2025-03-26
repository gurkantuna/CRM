using CRM.Core.Utilities.IoC.DotNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Core.Extensions {
    public static class ExtensionServices {
        public static IServiceCollection AddCoreDI(this IServiceCollection services, ICoreDIModule[] coreModules) {
            foreach(var coreModule in coreModules) {
                coreModule.Load(services);
            }
            return CoreServiceTool.Load(services);
        }

        public static void AddIdentityWithDbContext<TUser, TRole, TKey, TContext>(this IServiceCollection services)
                                                                                  where TUser : IdentityUser<TKey>
                                                                                  where TKey : IEquatable<TKey>
                                                                                  where TRole : IdentityRole<TKey>
                                                                                  where TContext : DbContext {

            services.AddDbContext<TContext>();
            services.AddIdentity<TUser, TRole>(options => {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<TContext>()
            .AddRoles<TRole>()            
            .AddDefaultTokenProviders();
        }

        public static void AddIdentity<TUser, TRole, TKey>(this IServiceCollection services)
                                                           where TUser : IdentityUser<TKey>
                                                           where TKey : IEquatable<TKey>
                                                           where TRole : IdentityRole<TKey> {

            services.AddIdentity<TUser, TRole>(options => options.User.RequireUniqueEmail = true)
                    .AddDefaultTokenProviders();
        }
    }
}
