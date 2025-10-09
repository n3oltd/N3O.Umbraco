using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingUrlProvider : DefaultUrlProvider {
    private readonly Lazy<ICrowdfundingUrlBuilder> _crowdfundingUrlBuilder;

    public CrowdfundingUrlProvider(IOptionsMonitor<RequestHandlerSettings> requestSettings,
                                   ILogger<DefaultUrlProvider> logger,
                                   ISiteDomainMapper siteDomainMapper,
                                   IUmbracoContextAccessor umbracoContextAccessor,
                                   UriUtility uriUtility,
                                   ILocalizationService localizationService,
                                   Lazy<ICrowdfundingUrlBuilder> crowdfundingUrlBuilder)
        : base(requestSettings, logger, siteDomainMapper, umbracoContextAccessor, uriUtility, localizationService) {
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
    }

    public override UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
        if (mode == UrlMode.Absolute &&
            content.ContentType.Alias.IsAnyOf(AliasHelper<CampaignContent>.ContentTypeAlias(),
                                              AliasHelper<FundraiserContent>.ContentTypeAlias())) {
            var url = content.ContentType.Alias == AliasHelper<CampaignContent>.ContentTypeAlias()
                          ? ViewCampaignPage.Url(_crowdfundingUrlBuilder.Value, content.Key)
                          : ViewEditFundraiserPage.Url(_crowdfundingUrlBuilder.Value, content.Key);
            
            return UrlInfo.Url(url, culture);
        }

        return base.GetUrl(content, mode, culture, current);
    }
}