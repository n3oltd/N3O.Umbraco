using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CrowdfundingCampaignPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly ICrowdfunderTemplatePublisher _crowdfunderTemplatePublisher;

    public CrowdfundingCampaignPublished(ISubscriptionAccessor subscriptionAccessor,
                                         ICloudUrl cloudUrl,
                                         IBackgroundJob backgroundJob,
                                         IContentTypeService contentTypeService,
                                         Lazy<IContentLocator> contentLocator,
                                         ILogger<CrowdfundingCampaignPublished> logger,
                                         ICrowdfunderTemplatePublisher crowdfunderTemplatePublisher)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
        _crowdfunderTemplatePublisher = crowdfunderTemplatePublisher;
    }
    
    protected override bool CanProcess(IContent content) {
        return content.IsCampaign(_contentTypeService) && CrowdfundingEnabled(content);
    }
    
    protected override object GetBody(IContent content) {
        var campaign = _contentLocator.Value.ById<CampaignContent>(content.Key);

        var req = new CrowdfundingCampaignWebhookBodyReq();
        
        req.CampaignId = campaign.Key.ToString();
        req.Action = WebhookSyncAction.AddOrUpdate;

        req.AddOrUpdate = GetCrowdfundingCampaignReq(campaign);
        
        return req;
    }

    private CrowdfundingCampaignReq GetCrowdfundingCampaignReq(CampaignContent campaign) {
        var req = new CrowdfundingCampaignReq();
        req.Activate = true;

        req.Template = new ContentReq();
        req.Template.SchemaAlias = CrowdfundingSystemSchema.Sys__crowdfunderPage.ToEnumString();
        req.Template.Properties = _crowdfunderTemplatePublisher.GetContentProperties(campaign.Content()).ToList();
        
        return req;
    }
    
    private bool CrowdfundingEnabled(IContent content) {
        return content.GetValue<bool>(AliasHelper<CampaignContent>.PropertyAlias(x => x.CrowdfundingEnabled));
    }

    protected override string HookId => PlatformsConstants.WebhookIds.CrowdfundingCampaigns;
}