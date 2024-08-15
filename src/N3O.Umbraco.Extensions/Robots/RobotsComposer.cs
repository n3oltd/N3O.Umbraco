using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions.Routing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Robots;

public class RobotsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IRobotsTxt, RobotsTxt>();
        
        // The Umbraco Context is not being set when a page is requested via an IVirtualPageController.
        // This OurPublishedRouter is a workaround until below issue is resolved.
        // https://github.com/umbraco/Umbraco-CMS/issues/12834#issue-1338670897
        // waiting on https://github.com/umbraco/Umbraco-CMS/pull/15121
        builder.Services.AddTransient<IPublishedRouter, OurPublishedRouter>();
        
        builder.Services.Configure<UmbracoPipelineOptions>(opt => {
            var filter = new UmbracoPipelineFilter(nameof(RobotsController));
            filter.Endpoints = app => app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute("Robots Controller",
                                             $"/{RobotsTxt.File}",
                                             new { Controller = "Robots", Action = "Index" });
            });
            
            opt.AddFilter(filter);
        });
    }
}
