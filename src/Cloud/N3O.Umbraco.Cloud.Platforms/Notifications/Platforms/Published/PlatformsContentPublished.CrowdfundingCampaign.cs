using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CrowdfundingCampaignPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IReadOnlyList<IPlatformsPageContentPublisher> _platformsPageContentPublishers;
    private readonly IUmbracoMapper _mapper;

    public CrowdfundingCampaignPublished(ISubscriptionAccessor subscriptionAccessor,
                                         ICloudUrl cloudUrl,
                                         IBackgroundJob backgroundJob,
                                         IContentTypeService contentTypeService,
                                         Lazy<IContentLocator> contentLocator,
                                         ILogger<CrowdfundingCampaignPublished> logger,
                                         IEnumerable<IPlatformsPageContentPublisher> platformsPageContentPublishers,
                                         IUmbracoMapper mapper)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
        _mapper = mapper;
        _platformsPageContentPublishers = platformsPageContentPublishers.ToList();
    }
    
    protected override bool CanProcess(IContent content) {
        return content.IsCrowdfundingCampaign(_contentTypeService) && CrowdfundingEnabled(content);
    }
    
    protected override object GetBody(IContent content) {
        var campaign = _contentLocator.Value.ById<CrowdfundingCampaignContent>(content.Key);

        var platformsPageContentPublisher = _platformsPageContentPublishers.GetPublisher(PlatformsSchemas.CrowdfunderPage);

        var req = _mapper.Map<CrowdfundingCampaignContent, CrowdfundingCampaignWebhookBodyReq>(campaign, ctx => {
            ctx.Items[CrowdfundingCampaignWebhookBodyReqMapping.PageContentContext] = platformsPageContentPublisher.GetContentProperties(campaign.Content());
        });
        
        return req;
    }
    
    private bool CrowdfundingEnabled(IContent content) {
        return content.GetValue<bool>(AliasHelper<CrowdfundingCampaignContent>.PropertyAlias(x => x.CrowdfundingEnabled));
    }

    protected override string HookId => PlatformsConstants.WebhookIds.CrowdfundingCampaigns;
}