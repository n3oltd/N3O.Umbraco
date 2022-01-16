using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Events.Content;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Events.ContentFinders {
    public class EventContentFinder : ContentFinderBase {
        public EventContentFinder(IContentCache contentCache) : base(contentCache) { }
        
        public override bool TryFindContentImpl(IPublishedRequestBuilder request) {
            return TryFindRelocatedContent(AliasHelper<EventsPage>.ContentTypeAlias(),
                                           AliasHelper<Event>.ContentTypeAlias(),
                                           AliasHelper<Content.Events>.ContentTypeAlias(),
                                           request);
        }
    }
}