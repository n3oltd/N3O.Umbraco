using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Hosting;
using Serilog;

namespace N3O.Umbraco.Monitoring.Sentry;

public class SentryHostBuilderExtension : IHostBuilderExtension {
    public void Run(IHostBuilder webBuilder) {
        webBuilder.UseSerilog((context, _, configuration) => {
            if (context.HostingEnvironment.IsProduction()) {
                configuration.ReadFrom
                             .Configuration(context.Configuration)
                             .Enrich.FromLogContext()
                             .WriteTo.Sentry();
            }
        });
    }
}