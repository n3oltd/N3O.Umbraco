using N3O.Umbraco.Cloud.Clients.Platforms;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CampaignPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public CampaignPublished(ISubscriptionAccessor subscriptionAccessor,
                             ICloudUrl cloudUrl,
                             IBackgroundJob backgroundJob,
                             IContentTypeService contentTypeService,
                             Lazy<IContentLocator> contentLocator,
                             IUmbracoMapper mapper)
        : base(subscriptionAccessor, cloudUrl, backgroundJob) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
        _mapper = mapper;
    }

    protected override object GetBody(IContent content) {
        var campaign = _contentLocator.Value.ById<CampaignContent>(content.Key);

        var campaignReq = _mapper.Map<CampaignContent, CampaignWebhookBodyReq>(campaign);

        return campaignReq;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsCampaign(_contentTypeService);
    }

    protected override string HookId => PlatformsConstants.WebhookIds.Campaigns;
}