using System;
using System.Threading;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PublishedCache;

namespace N3O.Umbraco.Extensions;

public static class PublishedContentCacheExtensions {
    private static readonly TimeSpan Delay = TimeSpan.FromMilliseconds(500);
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);
    
    public static void WaitForContentToAppearInCache(this IPublishedContentCache publishedContentCache,
                                                     IContent content) {
        for (var i = 0; i < (Timeout.TotalMilliseconds / Delay.TotalMilliseconds); i++) {
            try {
                var publishedContent = publishedContentCache.GetById(content.Key);

                if (content.UpdateDate == publishedContent?.UpdateDate) {
                    return;
                }
            } catch { }

            Thread.Sleep(Delay);
        }

        throw new Exception($"Content {content.Key} failed to appear in the cache within {Timeout.TotalSeconds} seconds");
    }
}