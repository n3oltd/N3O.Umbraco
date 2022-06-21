using Flurl;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace N3O.Umbraco.UrlProviders;

public abstract class UrlProvider : IUrlProvider {
    private readonly ILogger<UrlProvider> _logger;
    private readonly DefaultUrlProvider _defaultUrlProvider;
    private readonly IContentCache _contentCache;

    protected UrlProvider(ILogger<UrlProvider> logger,
                          DefaultUrlProvider defaultUrlProvider,
                          IContentCache contentCache) {
        _logger = logger;
        _defaultUrlProvider = defaultUrlProvider;
        _contentCache = contentCache;
    }

    public UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
        try {
            return ResolveUrl(content, mode, culture, current);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error executing GetUrl for URL provider");
            
            return null;
        }
    }

    public IEnumerable<UrlInfo> GetOtherUrls(int id, Uri current) {
        try {
            return ResolveOtherUrls(id, current);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error executing GetOtherUrls for URL provider");
            
            return Enumerable.Empty<UrlInfo>();
        }
    }

    protected virtual IEnumerable<UrlInfo> ResolveOtherUrls(int id, Uri current) => Enumerable.Empty<UrlInfo>();
    
    protected abstract UrlInfo ResolveUrl(IPublishedContent content, UrlMode mode, string culture, Uri current);
    
    protected UrlInfo TryGetRelocatedUrl(string pageTypeAlias,
                                         string contentTypeAlias,
                                         IPublishedContent content,
                                         UrlMode mode,
                                         string culture,
                                         Uri current) {
        if (content != null && content.ContentType.Alias.EqualsInvariant(contentTypeAlias)) {
            var page = _contentCache.Single(pageTypeAlias);

            if (page == null) {
                return null;
            }

            var defaultUrl = _defaultUrlProvider.GetUrl(page, mode, culture, current);
            var url = new Url(defaultUrl.Text);

            url.AppendPathSegment(content.UrlSegment);

            return UrlInfo.Url(url, culture);
        }

        return null;
    }

    protected UrlInfo TryGetRelocatedUrl(string pageTypeAlias,
                                         string contentTypeAlias,
                                         string contentCollectionTypeAlias,
                                         IPublishedContent content,
                                         UrlMode mode,
                                         string culture,
                                         Uri current) {
        if (content != null && content.ContentType.Alias.EqualsInvariant(contentTypeAlias)) {
            var collection = _contentCache.Single(contentCollectionTypeAlias);
            var page = _contentCache.Single(pageTypeAlias);

            if (collection == null || page == null) {
                return null;
            }

            var contentDefaultUrl = _defaultUrlProvider.GetUrl(content, mode, culture, current);

            return UrlInfo.Url(contentDefaultUrl.Text.Replace(collection.Url(), page.Url()), culture);
        }

        return null;
    }
}
