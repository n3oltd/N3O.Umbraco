using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Extensions {
    public static class ContentTypeServiceExtensions {
        public static IContentType GetContentTypeForContainerContent(this IContentTypeService contentTypeService,
                                                                     int containerContentTypeId) {
            return contentTypeService.Get(containerContentTypeId)
                                     .AllowedContentTypes
                                     .Select(x => contentTypeService.Get(x.Alias))
                                     .Single(x => !x.IsContainer);
        }
    }
}