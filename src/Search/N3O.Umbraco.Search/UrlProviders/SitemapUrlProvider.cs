using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Content;
using N3O.Umbraco.UrlProviders;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Search.UrlProviders {
    public class SitemapUrlProvider : UrlProviderBase {
        private static readonly string SitemapAlias = AliasHelper<SitemapContent>.ContentTypeAlias();

        public SitemapUrlProvider(DefaultUrlProvider defaultUrlProvider, IContentCache contentCache)
            : base(defaultUrlProvider, contentCache) { }
        
        public override UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
            if (content.ContentType.Alias.EqualsInvariant(SitemapAlias)) {
                return UrlInfo.Url($"/{SearchConstants.SitemapXml}", culture);
            }

            return null;
        }
    }
}