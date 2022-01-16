using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Blog.Content;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Blog.ContentFinders {
    public class BlogPostContentFinder : ContentFinderBase {
        public BlogPostContentFinder(IContentCache contentCache) : base(contentCache) { }
        
        public override bool TryFindContentImpl(IPublishedRequestBuilder request) {
            return TryFindRelocatedContent(AliasHelper<BlogPage>.ContentTypeAlias(),
                                           AliasHelper<BlogPost>.ContentTypeAlias(),
                                           AliasHelper<BlogPosts>.ContentTypeAlias(),
                                           request);
        }
    }
}