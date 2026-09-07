using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CrowdfunderPublished : CloudContentPublished {
    private readonly IContentHelper _contentHelper;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly ILogger<CrowdfunderPublished> _logger;
    private readonly IReadOnlyList<IPlatformsPageContentPublisher> _platformsPageContentPublishers;
    private readonly IUmbracoMapper _mapper;

    public CrowdfunderPublished(ICloudUrl cloudUrl,
                                IBackgroundJob backgroundJob,
                                IContentHelper contentHelper,
                                Lazy<IContentLocator> contentLocator,
                                ILogger<CrowdfunderPublished> logger,
                                IEnumerable<IPlatformsPageContentPublisher> platformsPageContentPublishers,
                                IUmbracoMapper mapper)
        : base(cloudUrl, backgroundJob, logger) {
        _contentHelper = contentHelper;
        _contentLocator = contentLocator;
        _logger = logger;
        _mapper = mapper;
        _platformsPageContentPublishers = platformsPageContentPublishers.ToList();
    }

    protected override bool CanProcess(IContent content) {
        if (!content.IsCrowdfunder()) {
            return false;
        }

        if (content.GetCrowdfunderCampaignKey(_contentHelper) == null) {
            _logger.LogWarning("Crowdfunding campaign {Name} ({Key}) has no campaign picked, so it was not sent",
                               content.Name,
                               content.Key);

            return false;
        }

        return true;
    }

    protected override Task<object> GetBodyAsync(IContent content) {
        var crowdfunder = _contentLocator.Value.ById<CrowdfunderContent>(content.Key);
        var campaignKey = content.GetCrowdfunderCampaignKey(_contentHelper).GetValueOrThrow();

        var crowdfunderPagePublisher = _platformsPageContentPublishers.GetPublisher(PlatformsSchemas.CrowdfunderPage);

        var campaignPagePublisher =
            _platformsPageContentPublishers.SingleOrDefault(x => x.IsPublisherFor(PlatformsSchemas.CrowdfundingCampaignPage));

        var req = _mapper.Map<CrowdfunderContent, CrowdfundingCampaignWebhookBodyReq>(crowdfunder, ctx => {
            ctx.Items[CrowdfunderWebhookBodyReqMapping.CampaignKeyContext] = campaignKey;

            ctx.Items[CrowdfunderWebhookBodyReqMapping.CrowdfunderPageContentContext] =
                crowdfunderPagePublisher.GetContentProperties(crowdfunder.Content());

            if (campaignPagePublisher != null) {
                ctx.Items[CrowdfunderWebhookBodyReqMapping.CampaignPageContentContext] =
                    campaignPagePublisher.GetContentProperties(crowdfunder.Content());
            }
        });

        return Task.FromResult<object>(req);
    }

    protected override string HookId => PlatformsConstants.Webhooks.HookIds.CrowdfundingCampaigns;
}
