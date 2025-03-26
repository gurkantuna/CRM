using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CRM.Core.Utilities.IoC.DotNetCore {
    public class CoreDIModule : ICoreDIModule {
        public CoreDIModule(IHostEnvironment? env = null, IConfiguration? conf = null) {
            _env = env;
            _conf = conf;
        }

        private readonly IConfiguration? _conf;
        private readonly IHostEnvironment? _env;

        public void Load(IServiceCollection services) {
            services.AddHttpsRedirection(options => {
                options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
                options.HttpsPort = 443;
            });

            //I:https://stackoverflow.com/a/39113342
            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            services.AddCors();

            if(_conf != null) {
                services.AddSingleton(_conf);
            }
        }
    }
}
