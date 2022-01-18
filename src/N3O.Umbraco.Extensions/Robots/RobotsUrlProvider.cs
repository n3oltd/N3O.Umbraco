using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.UrlProviders;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Robots {
    public class RobotsUrlProvider : UrlProviderBase {
        private static readonly string RobotsAlias = AliasHelper<RobotsContent>.ContentTypeAlias();

        public RobotsUrlProvider(DefaultUrlProvider defaultUrlProvider, IContentCache contentCache)
            : base(defaultUrlProvider, contentCache) { }
        
        public override UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
            if (content.ContentType.Alias.EqualsInvariant(RobotsAlias)) {
                return UrlInfo.Url($"/{RobotsTxt.File}", culture);
            }

            return null;
        }
    }
}