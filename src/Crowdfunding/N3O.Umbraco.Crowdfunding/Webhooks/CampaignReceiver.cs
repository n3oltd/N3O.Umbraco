using AsyncKeyedLock;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Extensions;
using N3O.Umbraco.Webhooks.Models;
using N3O.Umbraco.Webhooks.Receivers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Webhooks;

[WebhookReceiver(CrowdfundingConstants.Webhooks.HookIds.CampaignUrl)]
public class CampaignReceiver : WebhookReceiver {
    private readonly IContentService _contentService;
    private readonly IJsonProvider _jsonProvider;
    private readonly AsyncKeyedLocker<string> _locker;

    public CampaignReceiver(IJsonProvider jsonProvider,
                            AsyncKeyedLocker<string> locker,
                            IContentService contentService) {
        _jsonProvider = jsonProvider;
        _locker = locker;
        _contentService = contentService;
    }

    protected override async Task ProcessAsync(WebhookPayload payload, CancellationToken cancellationToken) {
        var campaignId = payload.RouteSegments.ElementAt(0).TryParseAs<Guid>().GetValueOrThrow();

        using (await _locker.LockAsync(campaignId.ToString(), cancellationToken)) {
            var campaign = _contentService.GetById(campaignId);

            if (campaign.HasValue()) {
                var productionCampaignUrl = payload.GetBody<string>(_jsonProvider);
                
                campaign.SetValue(CrowdfundingConstants.Campaign.Properties.ProductionUrl, productionCampaignUrl);
                
                _contentService.SaveAndPublish(campaign);
            }
        }
    }
}