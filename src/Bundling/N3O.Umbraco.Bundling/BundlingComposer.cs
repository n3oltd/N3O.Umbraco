using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Bundling {
    public class BundlingComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.Configure<UmbracoPipelineOptions>(opt => {
                var filter = new UmbracoPipelineFilter("Bundling");
                filter.Endpoints = app => app.UseWebOptimizer(WebHostEnvironment);
                
                opt.AddFilter(filter);
            });
        }
    }
}