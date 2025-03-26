using Microsoft.Extensions.DependencyInjection;

namespace CRM.Business.Utilities.IoC.DotNetCore {
    public static class BusinessServiceTool {
        public static IServiceProvider ServiceProvider { get; set; }

        public static IServiceCollection Load(IServiceCollection services) {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
