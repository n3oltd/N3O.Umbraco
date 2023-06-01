using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Profiling.Internal;
using System;
using System.Diagnostics;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Telemetry;

public class TelemetryComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        var config = new TelemetryConfiguration();

        builder.Config.GetSection("Tracing").Bind(config);

        if (config.ExporterUrl.HasValue()) {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IDurationWeightFinder, DurationWeightFinder>();

            builder.Services
                   .AddOpenTelemetry()
                   .WithTracing(b => {
                        if (config.Source.HasValue()) {
                            b.SetResourceBuilder(ResourceBuilder.CreateDefault()
                                                                .AddService(serviceName: config.Source));
                            b.AddSource(config.Source);
                        }
                        
                        b.AddConsoleExporter();
                        b.AddOtlpExporter(opt => opt.Endpoint = new Uri(config.ExporterUrl));
                        b.AddAspNetCoreInstrumentation();
                    });
        }

        if (config.Source.HasValue()) {
            builder.Services.AddSingleton(new ActivitySource(config.Source));
        }
    }
}
