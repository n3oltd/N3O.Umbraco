using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
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

public class CampaignPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;
    private readonly IReadOnlyList<ITemplatePublisher> _templatePublishers;

    public CampaignPublished(ISubscriptionAccessor subscriptionAccessor,
                             ICloudUrl cloudUrl,
                             IBackgroundJob backgroundJob,
                             IContentTypeService contentTypeService,
                             Lazy<IContentLocator> contentLocator,
                             IUmbracoMapper mapper,
                             ILogger<CampaignPublished> logger,
                             IEnumerable<ITemplatePublisher> templatePublishers)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
        _mapper = mapper;
        _templatePublishers = templatePublishers.ToList();
    }
    
    protected override bool CanProcess(IContent content) {
        return content.IsCampaign(_contentTypeService);
    }

    protected override object GetBody(IContent content) {
        var campaign = _contentLocator.Value.ById<CampaignContent>(content.Key);

        var templatePublisher = _templatePublishers.Single(x => x.IsPublisherFor(AliasHelper<CampaignContent>.ContentTypeAlias()));

        var campaignReq = _mapper.Map<CampaignContent, CampaignWebhookBodyReq>(campaign, ctx => {
            ctx.Items[UpdateCampaignReqMapping.PageContentContext] = templatePublisher.GetContentProperties(campaign.Content());                                                           
        });

        return campaignReq;
    }

    protected override string HookId => PlatformsConstants.WebhookIds.Campaigns;
}