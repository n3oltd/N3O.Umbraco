using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class PlatformsPageContentPublisherExtensions {
    public static IPlatformsPageContentPublisher GetPublisher(this IEnumerable<IPlatformsPageContentPublisher> platformsPageContentPublishers,
                                                              PlatformsSchema page) {
        var publisher = platformsPageContentPublishers.SingleOrDefault(x => x.IsPublisherFor(page));
        
        if (publisher == null) {
            throw new Exception($"Could not find a publisher for platforms page with schema ID {page.Id.Quote()}");
        }

        return publisher;
    }
}
