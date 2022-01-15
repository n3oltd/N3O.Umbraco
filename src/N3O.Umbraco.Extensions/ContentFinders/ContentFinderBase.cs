using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace N3O.Umbraco.ContentFinders {
    public abstract class ContentFinderBase : IContentFinder {
        protected ContentFinderBase(IContentCache contentCache) {
            ContentCache = contentCache;
        }

        public bool TryFindContent(IPublishedRequestBuilder request) {
            try {
                return TryFindContentImpl(request);
            } catch {
                return false;
            }
        }

        public abstract bool TryFindContentImpl(IPublishedRequestBuilder request);

        protected bool TryFindRelocatedContent(string pageTypeAlias,
                                               string contentTypeAlias,
                                               string contentCollectionTypeAlias,
                                               IPublishedRequestBuilder request) {
            var page = ContentCache.Single(pageTypeAlias);
            var contentCollection = ContentCache.Single(contentCollectionTypeAlias);

            if (page == null || contentCollection == null) {
                return false;
            }
        
            var pagePath = page.RelativeUrl();
        
            var path = GetRequestedPath(request.Uri, pagePath);

            if (path == null) {
                return false;
            }

            var match = contentCollection.Descendants()
                                         .Where(x => x.ContentType.Alias.EqualsInvariant(contentTypeAlias))
                                         .FirstOrDefault(x => StrippedPath(x, pagePath).EqualsInvariant(path));

            if (match != null) {
                request.SetPublishedContent(match);

                return true;
            }

            return false;
        }

        protected string StrippedPath(IPublishedContent content, string strip) {
            return content.RelativeUrl().Substring(strip.Length).RemoveLeadingSlashes();
        }

        protected string GetRequestedPath(Uri url, string strip) {
            var path = url.GetAbsolutePathDecoded().ToLowerInvariant();

            if (!path.StartsWith(strip.ToLowerInvariant())) {
                return null;
            }
        
            return path.Substring(strip.Length).RemoveLeadingSlashes();
        }
    
        protected IContentCache ContentCache { get; }
    }
}
