using Flurl;
using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace N3O.Umbraco.UrlProviders {
    public abstract class UrlProviderBase : IUrlProvider {
        private readonly DefaultUrlProvider _defaultUrlProvider;
        private readonly IContentCache _contentCache;

        protected UrlProviderBase(DefaultUrlProvider defaultUrlProvider, IContentCache contentCache) {
            _defaultUrlProvider = defaultUrlProvider;
            _contentCache = contentCache;
        }

        public abstract UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current);

        public virtual IEnumerable<UrlInfo> GetOtherUrls(int id, Uri current) => Enumerable.Empty<UrlInfo>();

        protected UrlInfo TryGetRelocatedUrl<TPage, TContent>(IPublishedContent content,
                                                              UrlMode mode,
                                                              string culture,
                                                              Uri current)
            where TPage : IPublishedContent
            where TContent : IPublishedContent {
            if (content != null && content.ContentType.Alias == AliasHelper<TContent>.ContentTypeAlias()) {
                var page = _contentCache.Single<TPage>();

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

        protected UrlInfo TryGetRelocatedUrl<TContentCollection, TPage, TContent>(IPublishedContent content,
                                                                                  UrlMode mode,
                                                                                  string culture,
                                                                                  Uri current)
            where TContentCollection : IPublishedContent
            where TPage : IPublishedContent
            where TContent : IPublishedContent {
            if (content != null && content.ContentType.Alias == AliasHelper<TContent>.ContentTypeAlias()) {
                var collection = _contentCache.Single<TContentCollection>();
                var page = _contentCache.Single<TPage>();

                if (collection == null || page == null) {
                    return null;
                }

                var contentDefaultUrl = _defaultUrlProvider.GetUrl(content, mode, culture, current);

                return UrlInfo.Url(contentDefaultUrl.Text.Replace(collection.Url(), page.Url()), culture);
            }

            return null;
        }
    }
}
