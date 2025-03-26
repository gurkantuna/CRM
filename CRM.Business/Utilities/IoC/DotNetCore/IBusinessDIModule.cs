using Microsoft.Extensions.DependencyInjection;

namespace CRM.Business.Utilities.IoC.DotNetCore {
    public interface IBusinessDIModule {
        void Load(IServiceCollection services);
    }
}
