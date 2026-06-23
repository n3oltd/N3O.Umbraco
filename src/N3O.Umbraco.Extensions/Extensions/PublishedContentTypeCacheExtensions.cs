using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Extensions;

public static class PublishedContentTypeCacheExtensions {
    public static IPublishedContentType Get(this IPublishedContentTypeCache publishedContentTypeCache,
                                            IContentTypeService contentTypeService,
                                            string contentTypeAlias) {
        var rawContentType = contentTypeService.Get(contentTypeAlias);

        if (rawContentType == null) {
            return null;
        }

        return publishedContentTypeCache.Get(rawContentType.GetItemType(), rawContentType.Alias);
    }
}