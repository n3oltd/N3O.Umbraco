using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Blog.Content;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Blog.ContentFinders {
    public class BlogPostContentFinder : ContentFinder {
        private static readonly string BlogPageAlias = AliasHelper<BlogPageContent>.ContentTypeAlias();
        private static readonly string BlogPostAlias = AliasHelper<BlogPostContent>.ContentTypeAlias();
        private static readonly string BlogPostsAlias = AliasHelper<BlogPostsContent>.ContentTypeAlias();
        
        public BlogPostContentFinder(ILogger<BlogPostContentFinder> logger, IContentCache contentCache)
            : base(logger, contentCache) { }

        protected override bool FindContent(IPublishedRequestBuilder request) {
            return TryFindRelocatedContent(BlogPageAlias, BlogPostAlias, BlogPostsAlias, request);
        }
    }
}