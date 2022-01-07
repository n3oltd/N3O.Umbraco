using System.Linq;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Extensions {
    public static class ContentTypeServiceExtensions {
        public static bool HasComposition(this IContentTypeService contentTypeService,
                                          string contentTypeAlias,
                                          string compositionAlias) {
            var contentType = contentTypeService.Get(contentTypeAlias);
            
            return contentType.ContentTypeComposition.Any(x => x.Alias.EqualsInvariant(compositionAlias));
        }
    }
}