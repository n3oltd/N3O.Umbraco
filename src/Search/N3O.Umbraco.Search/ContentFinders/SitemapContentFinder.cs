using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Content;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Search.ContentFinders {
    public class SitemapContentFinder : ContentFinder {
        private static readonly string SitemapAlias = AliasHelper<SitemapContent>.ContentTypeAlias();

        public SitemapContentFinder(ILogger<SitemapContentFinder> logger, IContentCache contentCache)
            : base(logger, contentCache) { }

        protected override bool FindContent(IPublishedRequestBuilder request) {
            var path = GetRequestedPath(request.Uri);

            if (path.EqualsInvariant(SearchConstants.SitemapXml)) {
                request.SetPublishedContent(ContentCache.Single(SitemapAlias));
                return true;
            }

            return false;
        }
    }
}