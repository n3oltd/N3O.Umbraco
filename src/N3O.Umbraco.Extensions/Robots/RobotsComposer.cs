using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Robots {
    public class RobotsComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<IRobotsTxt, RobotsTxt>();
            
            builder.Services.Configure<UmbracoPipelineOptions>(options => {
                var filter = new UmbracoPipelineFilter(nameof(RobotsController));
                filter.Endpoints = app => app.UseEndpoints(endpoints => {
                    endpoints.MapControllerRoute("Robots Controller",
                                                 $"/{RobotsTxt.File}",
                                                 new { Controller = "Robots", Action = "Index" });
                });
                
                options.AddFilter(filter);
            });
        }
    }
}