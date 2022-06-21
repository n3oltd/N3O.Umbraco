using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Blog.Content;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Blog.ContentFinders;

public class BlogPostContentFinder : ContentFinder {
    private static readonly string BlogPageAlias = AliasHelper<BlogPageContent>.ContentTypeAlias();
    private static readonly string BlogPostAlias = AliasHelper<BlogPostContent>.ContentTypeAlias();
    private static readonly string BlogPostsAlias = AliasHelper<BlogPostsContent>.ContentTypeAlias();
    
    public BlogPostContentFinder(ILogger<BlogPostContentFinder> logger, IContentCache contentCache)
        : base(logger, contentCache) { }

    protected override Task<bool> FindContentAsync(IPublishedRequestBuilder request) {
        return TryFindRelocatedContentAsync(BlogPageAlias, BlogPostAlias, BlogPostsAlias, request);
    }
}
