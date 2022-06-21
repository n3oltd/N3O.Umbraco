using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Events.Content;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UrlProvider = N3O.Umbraco.UrlProviders.UrlProvider;

namespace N3O.Umbraco.Events.UrlProviders;

public class EventUrlProvider : UrlProvider {
    private static readonly string EventsPageAlias = AliasHelper<EventsPageContent>.ContentTypeAlias();
    private static readonly string EventAlias = AliasHelper<EventContent>.ContentTypeAlias();
    
    public EventUrlProvider(ILogger<EventUrlProvider> logger,
                            DefaultUrlProvider defaultUrlProvider,
                            IContentCache contentCache)
        : base(logger, defaultUrlProvider, contentCache) { }

    protected override UrlInfo ResolveUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
        return TryGetRelocatedUrl(EventsPageAlias, EventAlias, content, mode, culture, current);
    }
}
