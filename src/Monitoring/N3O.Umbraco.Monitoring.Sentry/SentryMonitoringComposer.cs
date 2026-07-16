using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Monitoring.Sentry.Configuration;
using N3O.Umbraco.Monitoring.Sentry.Extensions;
using Sentry.Extensibility;
using Serilog.Configuration;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Monitoring.Sentry;

public class SentryMonitoringComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<ISentryEventProcessor, OurEventProcessor>();

        if (WebHostEnvironment.IsSentryEnabled()) {
            var configuration = new SentryConfiguration();

            builder.Config.GetSection("Sentry").Bind(configuration);

            builder.Services.AddSingleton<ILoggerSettings>(new SentryLoggerSettings(configuration,
                                                                                    WebHostEnvironment.EnvironmentName));
        }

        if (WebHostEnvironment.IsProduction()) {
            builder.Services.Configure<UmbracoPipelineOptions>(opt => {
                var filter = new UmbracoPipelineFilter("SentryMonitoring");
                filter.Endpoints = app => app.UseSentryTracing();

                opt.AddFilter(filter);
            });
        }
    }
}
