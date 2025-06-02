using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Telemetry;

public class TelemetryComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(TelemetryConstants.ApiName);
        
        builder.Services.AddTransient<ITelemetryData, TelemetryData>();
    }
}