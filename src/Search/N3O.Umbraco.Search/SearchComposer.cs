using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Search;

public class SearchComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<ISitemap, Sitemap>();
        
        RegisterAll(t => t.ImplementsInterface<ISitemapEntriesProvider>(),
                    t => builder.Services.AddTransient(typeof(ISitemapEntriesProvider), t));
    }
}