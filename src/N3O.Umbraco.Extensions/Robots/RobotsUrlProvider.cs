using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UrlProvider = N3O.Umbraco.UrlProviders.UrlProvider;

namespace N3O.Umbraco.Robots;

public class RobotsUrlProvider : UrlProvider {
    private static readonly string RobotsAlias = AliasHelper<RobotsContent>.ContentTypeAlias();

    public RobotsUrlProvider(ILogger<RobotsUrlProvider> logger,
                             DefaultUrlProvider defaultUrlProvider,
                             IContentCache contentCache)
        : base(logger, defaultUrlProvider, contentCache) { }

    protected override UrlInfo ResolveUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
        if (content.ContentType.Alias.EqualsInvariant(RobotsAlias)) {
            return UrlInfo.Url($"/{RobotsTxt.File}", culture);
        }

        return null;
    }
}
