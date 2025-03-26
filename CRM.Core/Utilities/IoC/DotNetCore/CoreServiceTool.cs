using Microsoft.Extensions.DependencyInjection;

namespace CRM.Core.Utilities.IoC.DotNetCore {
    public static class CoreServiceTool {
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// CoreServiceTool static sınıfı ServiceProvider property üzerinden, ayağa kaldırılmış olan servislere erişim 
        /// </summary>
        /// <param name="services">Daha sonra ulaşılması istenen servisler</param>
        /// <returns>IServiceCollection</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IServiceCollection Load(IServiceCollection services) {
            ServiceProvider = services.BuildServiceProvider();

            return ServiceProvider == null
                ? throw new InvalidOperationException("Core servislerin ayağa kalkabilmesiiçin Execute edilen projenin DI servislerine AddCoreDI() metodu eklenmeli")
                : services;
        }
    }
}
