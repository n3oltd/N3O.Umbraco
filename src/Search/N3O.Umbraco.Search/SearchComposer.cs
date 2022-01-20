using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Search.Controllers;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Search {
    public class SearchComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<ISitemap, Sitemap>();
            
            builder.Services.Configure<UmbracoPipelineOptions>(opt => {
                var filter = new UmbracoPipelineFilter(nameof(SitemapController));
                filter.Endpoints = app => app.UseEndpoints(endpoints => {
                    endpoints.MapControllerRoute("Sitemap Controller",
                                                 $"/{SearchConstants.SitemapXml}",
                                                 new { Controller = "Sitemap", Action = "Index" });
                });
                
                opt.AddFilter(filter);
            });
        }
    }
}