using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Blog.Content;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UrlProvider = N3O.Umbraco.UrlProviders.UrlProvider;

namespace N3O.Umbraco.Blog.UrlProviders;

public class BlogPostUrlProvider : UrlProvider {
    private static readonly string BlogPageAlias = AliasHelper<BlogPageContent>.ContentTypeAlias();
    private static readonly string BlogPostAlias = AliasHelper<BlogPostContent>.ContentTypeAlias();

    public BlogPostUrlProvider(ILogger<BlogPostUrlProvider> logger,
                               DefaultUrlProvider defaultUrlProvider,
                               IContentCache contentCache)
        : base(logger, defaultUrlProvider, contentCache) { }

    protected override UrlInfo ResolveUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
        return TryGetRelocatedUrl(BlogPageAlias, BlogPostAlias, content, mode, culture, current);
    }
}
