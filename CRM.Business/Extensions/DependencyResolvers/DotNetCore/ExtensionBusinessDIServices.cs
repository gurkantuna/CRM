using CRM.Business.Utilities.IoC.DotNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Business.Extensions.DependencyResolvers.DotNetCore {
    public static class ExtensionBusinessDIServices {
        public static IServiceCollection AddBusinessDI(this IServiceCollection services, IBusinessDIModule[] businessDependencyInjectionModulesModules) {

            foreach(var businessDependencyInjectionModule in businessDependencyInjectionModulesModules) {
                businessDependencyInjectionModule.Load(services);
            }

            return BusinessServiceTool.Load(services);
        }
    }
}