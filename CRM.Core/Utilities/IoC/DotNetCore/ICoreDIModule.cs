using Microsoft.Extensions.DependencyInjection;

namespace CRM.Core.Utilities.IoC.DotNetCore {
    public interface ICoreDIModule {
        void Load(IServiceCollection services);
    }
}