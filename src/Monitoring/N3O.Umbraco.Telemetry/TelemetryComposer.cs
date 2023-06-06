using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Telemetry.Configuration;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Telemetry;

public class TelemetryComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        var config = new TelemetryConfiguration();

        builder.Config.GetSection("Telemetry").Bind(config);

        if (config.Enabled) {
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<ITelemetryStopwatch, TelemetryStopwatch>();

            builder.Services
                   .AddOpenTelemetry()
                   .WithTracing(tracerProviderBuilder => {
                       tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault()
                                                                               .AddService(config.ServiceName));

                       tracerProviderBuilder.AddSource("N3O.*");

                       if (config.CustomActivitySources.HasAny()) {
                           tracerProviderBuilder.AddSource(config.CustomActivitySources.ToArray());
                       }

                       if (config.OtlpExporterUrl.HasValue()) {
                           tracerProviderBuilder.AddOtlpExporter(opt => opt.Endpoint = new Uri(config.OtlpExporterUrl));
                       }
                       
                       if (config.UseConsoleExporter) {
                           tracerProviderBuilder.AddConsoleExporter();
                       }

                       tracerProviderBuilder.AddAspNetCoreInstrumentation();
                   });
        }
    }
}