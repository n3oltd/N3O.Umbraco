using N3O.Umbraco.Content;
using N3O.Umbraco.Events.Content;
using N3O.Umbraco.UrlProviders;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Events.UrlProviders {
    public class EventUrlProvider : UrlProviderBase {
        private static readonly string EventsPageAlias = AliasHelper<EventsPageContent>.ContentTypeAlias();
        private static readonly string EventAlias = AliasHelper<EventContent>.ContentTypeAlias();
        
        public EventUrlProvider(DefaultUrlProvider defaultUrlProvider, IContentCache contentCache)
            : base(defaultUrlProvider, contentCache) { }
        
        public override UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
            return TryGetRelocatedUrl(EventsPageAlias, EventAlias, content, mode, culture, current);
        }
    }
}