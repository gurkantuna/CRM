using CRM.Business.Abstract;
using CRM.Business.Concrete;
using CRM.Business.Infrastructer.EntitiyFrameWorkCore;
using CRM.Business.Managers;
using CRM.Core.Business.UnitOfWork.Abstract;
using CRM.Core.Managers.Abstract;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Business.Utilities.IoC.DotNetCore {
    public class BusinessDIModule : IBusinessDIModule {

        public void Load(IServiceCollection services) {
            services.AddScoped<IUnitOfWork<CrmDbContext>, CRMUnitOfWork>();
            services.AddScoped<IDbControlManager<CrmDbContext>, CrmDbControlManager>();

            services.AddScoped<ICustomerBusiness, CustomerBusiness>();
            services.AddScoped<IRegionBusiness, RegionBusiness>();
            services.AddScoped<ILogBusiness, LogBusiness>();
            services.AddScoped<IDbLogger, DbLogger>();

            services.AddLogging();

            services.ConfigureApplicationCookie(option => {
                option.LoginPath = new PathString("/admin/account");
                option.LogoutPath = new PathString("/admin/account/signout");
                option.Cookie = new CookieBuilder {
                    Name = "AspNetCoreIdentity",
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.Always
                };
                option.SlidingExpiration = true;
                option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            });
        }
    }
}