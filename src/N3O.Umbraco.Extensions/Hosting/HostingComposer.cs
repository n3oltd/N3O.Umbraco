using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Cms.Web.Website.Controllers;

namespace N3O.Umbraco.Hosting {
    public class HostingComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.Configure<UmbracoRenderingDefaultsOptions>(c => {
                c.DefaultControllerType = typeof(PageController);
            });
        
            builder.Services.AddTransient<IConfigureOptions<MvcOptions>, OurMvcBinderOptions>();
            builder.Services.AddTransient<IConfigureOptions<MvcOptions>, OurCacheProfileOptions>();
            builder.Services.AddScoped<IActionLinkGenerator, ActionLinkGenerator>();
            builder.Services.AddScoped<StagingMiddleware>();

            builder.Services.AddOpenApiDocument("DevTools");
            
            builder.Services.Configure<UmbracoPipelineOptions>(options => {
                var filter = new UmbracoPipelineFilter("Staging");
                filter.PrePipeline = app => {
                    var runtimeState = app.ApplicationServices.GetRequiredService<IRuntimeState>();

                    if (runtimeState.Level == RuntimeLevel.Run) {
                        app.UseMiddleware<StagingMiddleware>();
                    }
                };

                options.AddFilter(filter);
            });
        }
    }
}
