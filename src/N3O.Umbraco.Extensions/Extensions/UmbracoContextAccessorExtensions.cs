using System;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Extensions; 

public static class UmbracoContextAccessorExtensions {
    public static IPublishedContentCache GetContentCache(this IUmbracoContextAccessor umbracoContextAccessor) {
        return GetUmbracoContext(umbracoContextAccessor).Content;
    }

    public static IUmbracoContext GetUmbracoContext(this IUmbracoContextAccessor umbracoContextAccessor) {
        if (!umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext)) {
            throw new Exception("No Umbraco context is available");
        }

        return umbracoContext;
    }
}