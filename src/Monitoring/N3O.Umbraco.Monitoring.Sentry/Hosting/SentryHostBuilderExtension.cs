using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Monitoring.Sentry.Configuration;
using Serilog;

namespace N3O.Umbraco.Monitoring.Sentry;

public class SentryHostBuilderExtension : IHostBuilderExtension {
    public void Run(IHostBuilder webBuilder) {
        webBuilder.UseSerilog((context, _, configuration) => {
            if (context.HostingEnvironment.IsProduction()) {
                var config = new SentryConfiguration();

                context.Configuration.GetSection("Sentry").Bind(config);

                configuration.ReadFrom
                             .Configuration(context.Configuration)
                             .Enrich.FromLogContext()
                             .WriteTo.Sentry(opt => {
                                 opt.InitializeSdk = false;
                                 opt.Dsn = config.Dsn;
                                 opt.Environment = context.HostingEnvironment.EnvironmentName;
                             });
            }
        });
    }
}