using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Content;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UrlProvider = N3O.Umbraco.UrlProviders.UrlProvider;

namespace N3O.Umbraco.Search.UrlProviders;

public class SitemapUrlProvider : UrlProvider {
    private static readonly string SitemapAlias = AliasHelper<SitemapContent>.ContentTypeAlias();

    public SitemapUrlProvider(ILogger<SitemapUrlProvider> logger,
                              DefaultUrlProvider defaultUrlProvider,
                              IContentCache contentCache)
        : base(logger, defaultUrlProvider, contentCache) { }

    protected override UrlInfo ResolveUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
        if (content.ContentType.Alias.EqualsInvariant(SitemapAlias)) {
            return UrlInfo.Url($"/{SearchConstants.SitemapXml}", culture);
        }

        return null;
    }
}
