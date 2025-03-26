using CRM.Business.Extensions.DependencyResolvers.DotNetCore;
using CRM.Business.Utilities.IoC.DotNetCore;
using CRM.Core.Constants;
using CRM.Core.Extensions;
using CRM.Core.Utilities.IoC.DotNetCore;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;
using CRM.Entity.Concrete;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);
builder.Services.AddIdentityWithDbContext<User, IdentityRole<int>, int, CrmDbContext>();
builder.Services.AddCoreDI([new CoreDIModule(builder.Environment, builder.Configuration)]);
builder.Services.AddBusinessDI([new BusinessDIModule()]);

builder.Logging.AddConsole();

var app = builder.Build();

if(app.Environment.IsDevelopment()) {
    app.UseStatusCodePages();
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePagesWithReExecute($"/{Strings.Error}", $"?{Strings.status}=" + "{0}");   
}
else {
    app.UseExceptionHandler($"/{Strings.Error}");
    app.UseStatusCodePagesWithReExecute($"/{Strings.Error}", $"?{Strings.status}=" + "{0}");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors(policy => policy.WithOrigins("https://localhost:5001")
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyHeader()
                            .AllowAnyMethod());

app.UseCookiePolicy(new CookiePolicyOptions {
    MinimumSameSitePolicy = SameSiteMode.None,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: Strings.admin,
        pattern: "{area:exists}/{controller=Account}/{action=Index}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
#pragma warning restore ASP0014

app.Run();
