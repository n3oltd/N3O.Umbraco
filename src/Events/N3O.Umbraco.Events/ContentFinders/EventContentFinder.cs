using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Events.Content;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Events.ContentFinders;

public class EventContentFinder : ContentFinder {
    private static readonly string EventsPageAlias = AliasHelper<EventsPageContent>.ContentTypeAlias();
    private static readonly string EventAlias = AliasHelper<EventContent>.ContentTypeAlias();
    private static readonly string EventsAlias = AliasHelper<EventsContent>.ContentTypeAlias();
    
    public EventContentFinder(ILogger<EventContentFinder> logger, IContentCache contentCache)
        : base(logger, contentCache) { }

    protected override Task<bool> FindContentAsync(IPublishedRequestBuilder request) {
        return TryFindRelocatedContentAsync(EventsPageAlias, EventAlias, EventsAlias, request);
    }
}
