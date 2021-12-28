using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Monitoring.Sentry {
    public class SentryWebHostBuilderExtension : IWebHostBuilderExtension {
        public void Run(IWebHostBuilder webBuilder) {
            webBuilder.ConfigureServices((ctx, _) => {
                if (ctx.HostingEnvironment.IsProduction()) {
                    webBuilder.UseSentry(opt => {
                        opt.AttachStacktrace = true;
                        opt.Environment = "Production";
                    });     
                }
            });
        }
    }
}