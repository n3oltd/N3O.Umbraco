using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Redirects;

public class RedirectsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IRedirectManagement, RedirectManagement>();
        
        RegisterMiddleware(builder);
    }
    
    private void RegisterMiddleware(IUmbracoBuilder builder) {
        /*builder.Services.AddScoped<RedirectMiddleware>();
        
        builder.Services.Configure<UmbracoPipelineOptions>(opt => {
            var filter = new UmbracoPipelineFilter(nameof(RedirectMiddleware));

            filter.PrePipeline = app => {
                var runtimeState = app.ApplicationServices.GetRequiredService<IRuntimeState>();

                if (runtimeState.Level == RuntimeLevel.Run) {
                    app.UseMiddleware<RedirectMiddleware>();
                }
            };

            opt.PipelineFilters.Insert(0, filter);
        });*/
    }
}
