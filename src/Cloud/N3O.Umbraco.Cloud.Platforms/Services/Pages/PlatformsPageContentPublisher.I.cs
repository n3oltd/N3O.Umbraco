using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public interface IPlatformsPageContentPublisher {
    bool IsPublisherFor(PlatformsSchema page);
    IEnumerable<PropertyContentReq> GetContentProperties(IPublishedContent publishedContent);
}