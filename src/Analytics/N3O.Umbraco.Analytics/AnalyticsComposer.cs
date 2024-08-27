using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Analytics;

public class AnalyticsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IDataLayerBuilder, DataLayerBuilder>();
        builder.Services.AddTransient<IAttributionAccessor, AttributionAccessor>();
        
        RegisterAll(t => t.ImplementsInterface<IDataLayerProvider>(),
                    t => builder.Services.AddTransient(typeof(IDataLayerProvider), t));
    }
}
