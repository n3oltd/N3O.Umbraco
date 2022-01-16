using N3O.Umbraco.Content;
using N3O.Umbraco.Blog.Content;
using N3O.Umbraco.UrlProviders;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Blog.UrlProviders {
    public class BlogPostUrlProvider : UrlProviderBase {
        private static readonly string BlogPageAlias = AliasHelper<BlogPageContent>.ContentTypeAlias();
        private static readonly string BlogPostAlias = AliasHelper<BlogPostContent>.ContentTypeAlias();

        public BlogPostUrlProvider(DefaultUrlProvider defaultUrlProvider, IContentCache contentCache)
            : base(defaultUrlProvider, contentCache) { }
        
        public override UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
            return TryGetRelocatedUrl(BlogPageAlias, BlogPostAlias, content, mode, culture, current);
        }
    }
}