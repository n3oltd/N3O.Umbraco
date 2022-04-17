using Algolia.Search.Clients;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Search.Algolia.Content;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Search.Algolia {
    public class AlgoliaSearchComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<ISearcher, AlgoliaSearcher>();
            
            builder.Services.AddSingleton<ISearchClient>(serviceProvider => {
                var contentCache = serviceProvider.GetRequiredService<IContentCache>();
                var settings = contentCache.Single<AlgoliaSettingsContent>();

                return new SearchClient(settings.ApplicationId, settings.ApiKey);
            });
        }
    }
}
