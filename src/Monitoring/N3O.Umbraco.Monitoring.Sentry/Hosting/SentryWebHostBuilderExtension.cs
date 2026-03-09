using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Monitoring.Sentry;

public class SentryWebHostBuilderExtension : IWebHostBuilderExtension {
    public void Run(IWebHostBuilder webBuilder) {
        if (Composer.WebHostEnvironment.IsProduction()) {
            webBuilder.UseSentry(opt => {
                opt.InitializeSdk = false;
                opt.MinimumEventLevel = LogLevel.Error;
                opt.MinimumBreadcrumbLevel = LogLevel.Error;
            });
        }
    }
}