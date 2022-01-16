using N3O.Umbraco.Content;
using N3O.Umbraco.Events.Content;
using N3O.Umbraco.UrlProviders;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Events.UrlProviders {
    public class EventUrlProvider : UrlProviderBase {
        public EventUrlProvider(DefaultUrlProvider defaultUrlProvider, IContentCache contentCache)
            : base(defaultUrlProvider, contentCache) { }
        
        public override UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
            return TryGetRelocatedUrl(AliasHelper<EventsPage>.ContentTypeAlias(),
                                      AliasHelper<Event>.ContentTypeAlias(),
                                      content,
                                      mode,
                                      culture,
                                      current);
        }
    }
}