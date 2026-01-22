using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CrowdfundingCampaignPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public CrowdfundingCampaignPublished(ISubscriptionAccessor subscriptionAccessor,
                                         ICloudUrl cloudUrl,
                                         IBackgroundJob backgroundJob,
                                         IContentTypeService contentTypeService,
                                         Lazy<IContentLocator> contentLocator,
                                         IUmbracoMapper mapper,
                                         ILogger<CrowdfundingCampaignPublished> logger)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
        _mapper = mapper;
    }
    
    protected override bool CanProcess(IContent content) {
        return content.IsCampaign(_contentTypeService) && CrowdfundingEnabled(content);
    }
    
    protected override object GetBody(IContent content) {
        var campaign = _contentLocator.Value.ById<CampaignContent>(content.Key);

        var crowdfundingReq = _mapper.Map<CampaignContent, CrowdfundingCampaignWebhookBodyReq>(campaign);

        return crowdfundingReq;
    }
    
    private bool CrowdfundingEnabled(IContent content) {
        return content.GetValue<bool>(AliasHelper<CampaignContent>.PropertyAlias(x => x.CrowdfundingEnabled));
    }

    protected override string HookId => PlatformsConstants.WebhookIds.CrowdfundingCampaigns;
}