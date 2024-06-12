using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Analytics.Hosting;
using N3O.Umbraco.Analytics.Services;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Validation;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Analytics;

public class AnalyticsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IDataLayerProvider>(),
                    t => builder.Services.AddTransient(typeof(IDataLayerProvider), t));

        builder.Services.AddTransient<IDataLayerBuilder, DataLayerBuilder>();
        builder.Services.AddTransient<IAttributionAccessor, AttributionAccessor>();
        builder.Services.AddTransient<IAttributionHelper, AttributionHelper>();
        
        RegisterMiddleware(builder);
    }
    
    private void RegisterMiddleware(IUmbracoBuilder builder) {
        builder.Services.AddScoped<AttributionMiddleware>();
        
        builder.Services.Configure<UmbracoPipelineOptions>(opt => {
            var filter = new UmbracoPipelineFilter(nameof(ExceptionMiddleware));

            filter.PrePipeline = app => {
                var runtimeState = app.ApplicationServices.GetRequiredService<IRuntimeState>();

                if (runtimeState.Level == RuntimeLevel.Run) {
                    app.UseMiddleware<AttributionMiddleware>();
                }
            };

            opt.AddFilter(filter);
        });
    }
}
