using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Monitoring.Sentry.Configuration;
using Sentry;
using Sentry.Extensibility;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Monitoring.Sentry;

public class SentryMonitoringComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<ISentryEventProcessor, OurEventProcessor>();

        if (WebHostEnvironment.IsProduction()) {
            var config = new SentryConfiguration();

            builder.Config.GetSection("Sentry").Bind(config);
            
            SentrySdk.Init(opt => {
                opt.Dsn = config.Dsn;
                opt.ReportAssembliesMode = ReportAssembliesMode.InformationalVersion;
                opt.SendDefaultPii = true;
                opt.Environment = WebHostEnvironment.EnvironmentName;
                opt.DiagnosticLevel = SentryLevel.Error;
                opt.TracesSampleRate = 1.0f; // TODO confirm what value to use or want to use tracesampler
            });
            
            builder.Services.Configure<UmbracoPipelineOptions>(opt => {
                var filter = new UmbracoPipelineFilter("SentryMonitoring");
                filter.Endpoints = app => app.UseSentryTracing();

                opt.AddFilter(filter);
            });
        }
    }
}
