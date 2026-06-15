using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;

namespace N3O.Umbraco.Extensions;

public static class DocumentNavigationQueryServiceExtensions {
    public static IEnumerable<IPublishedContent> GetPublishedRootContents(this INavigationQueryService navigationQueryService,
                                                                          IPublishedCache publishedCache) {
        navigationQueryService.TryGetRootKeys(out var rootKeys);
        
        var rootContents = new List<IPublishedContent>();
                    
        foreach (var rootKey in rootKeys) {
            var root = publishedCache.GetById(rootKey);

            if (root != null) {
                rootContents.Add(root);
            }
        }

        return rootContents;
    }
}