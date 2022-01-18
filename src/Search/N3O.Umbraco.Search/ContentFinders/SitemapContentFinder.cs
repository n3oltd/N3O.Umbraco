using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Content;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Search.ContentFinders {
    public class SitemapContentFinder : ContentFinderBase {
        private static readonly string SitemapAlias = AliasHelper<SitemapContent>.ContentTypeAlias();

        public SitemapContentFinder(IContentCache contentCache) : base(contentCache) { }
        
        public override bool TryFindContentImpl(IPublishedRequestBuilder request) {
            var path = GetRequestedPath(request.Uri);

            if (path.EqualsInvariant(SearchConstants.SitemapXml)) {
                request.SetPublishedContent(ContentCache.Single(SitemapAlias));
                return true;
            }

            return false;
        }
    }
}