using N3O.Umbraco.Content;
using N3O.Umbraco.Blog.Content;
using N3O.Umbraco.UrlProviders;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Blog.UrlProviders {
    public class BlogPostUrlProvider : UrlProviderBase {
        public BlogPostUrlProvider(DefaultUrlProvider defaultUrlProvider, IContentCache contentCache)
            : base(defaultUrlProvider, contentCache) { }
        
        public override UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
            return TryGetRelocatedUrl(AliasHelper<BlogPage>.ContentTypeAlias(),
                                      AliasHelper<BlogPost>.ContentTypeAlias(),
                                      content,
                                      mode,
                                      culture,
                                      current);
        }
    }
}