using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Monitoring.Sentry;

public class SentryWebHostBuilderExtension : IWebHostBuilderExtension {
    public void Run(IWebHostBuilder webBuilder) {
        if (webBuilder.GetSetting(WebHostDefaults.EnvironmentKey).EqualsInvariant(Environments.Production)) {
            webBuilder.UseSentry(opt => {
                opt.AttachStacktrace = true;
                opt.Environment = Composer.WebHostEnvironment.EnvironmentName;
            });
        }
    }
}
