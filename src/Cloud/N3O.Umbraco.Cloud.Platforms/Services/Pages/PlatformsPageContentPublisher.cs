using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class PlatformsPageContentPublisher : IPlatformsPageContentPublisher {
    public abstract bool IsPublisherFor(PlatformsSchema page);

    public abstract IEnumerable<PropertyContentReq> GetContentProperties(IPublishedContent publishedContent);
}

public abstract class PlatformsPageContentPublisher<T> : PlatformsPageContentPublisher where T : IPublishedContent {
    public override IEnumerable<PropertyContentReq> GetContentProperties(IPublishedContent publishedContent) {
        return GetContentProperties((T) publishedContent);
    }
    
    protected abstract IEnumerable<PropertyContentReq> GetContentProperties(T publishedContent);
}