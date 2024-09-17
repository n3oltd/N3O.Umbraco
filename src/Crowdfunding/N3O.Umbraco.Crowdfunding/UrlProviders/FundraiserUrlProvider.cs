using Flurl;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;
using UrlProvider = N3O.Umbraco.UrlProviders.UrlProvider;

namespace N3O.Umbraco.Crowdfunding;

public class FundraiserUrlProvider : UrlProvider {
    private readonly IContentCache _contentCache;

    public FundraiserUrlProvider(ILogger<UrlProvider> logger,
                                 DefaultUrlProvider defaultUrlProvider,
                                 IContentCache contentCache)
        : base(logger, defaultUrlProvider, contentCache) {
        _contentCache = contentCache;
    }

    protected override UrlInfo ResolveUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
        if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias)) {
            var homePage = _contentCache.Single(CrowdfundingConstants.HomePage.Alias);
            var fundraiser = content.As<FundraiserContent>();

            var url = new Url(homePage.Url(culture, mode));

            url.AppendPathSegment("pages");
            url.AppendPathSegment(fundraiser.Content().Id);
            url.AppendPathSegment($"{fundraiser.Slug}/");
            
            return UrlInfo.Url(url, culture);
        } else {
            return null;
        }
    }
}