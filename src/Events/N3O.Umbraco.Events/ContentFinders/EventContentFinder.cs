using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Events.Content;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Events.ContentFinders {
    public class EventContentFinder : ContentFinderBase {
        private static readonly string EventsPageAlias = AliasHelper<EventsPageContent>.ContentTypeAlias();
        private static readonly string EventAlias = AliasHelper<EventContent>.ContentTypeAlias();
        private static readonly string EventsAlias = AliasHelper<EventsContent>.ContentTypeAlias();
        
        public EventContentFinder(IContentCache contentCache) : base(contentCache) { }
        
        public override bool TryFindContentImpl(IPublishedRequestBuilder request) {
            return TryFindRelocatedContent(EventsPageAlias, EventAlias, EventsAlias, request);
        }
    }
}