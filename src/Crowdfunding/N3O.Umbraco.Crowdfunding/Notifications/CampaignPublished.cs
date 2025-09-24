using Flurl;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Engage;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using N3O.Umbraco.Webhooks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

[SkipDuringSync]
public class CampaignPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfunderManager _crowdfunderManager;

    public CampaignPublished(IContentLocator contentLocator, ICrowdfunderManager crowdfunderManager) {
        _contentLocator = contentLocator;
        _crowdfunderManager = crowdfunderManager;
    }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias)) {
                var campaign = _contentLocator.ById<CampaignContent>(content.Key);
                var urlSettingsContent = _contentLocator.Single<UrlSettingsContent>();
        
                if (!campaign.Status.HasValue()) {
                    await _crowdfunderManager.CreateCampaignAsync(campaign, GetWebhookUrls(urlSettingsContent));
                } else {
                    await _crowdfunderManager.UpdateCrowdfunderAsync(campaign.Key.ToString(),
                                                                     campaign,
                                                                     campaign.ToggleStatus,
                                                                     GetWebhookUrls(urlSettingsContent));
                }
            }
        }
    }
    
    private IEnumerable<string> GetWebhookUrls(UrlSettingsContent urlSettingsContent) {
        var webhookUrls = new List<string>();
        
        webhookUrls.Add(GetWebhookUrl(urlSettingsContent.ProductionBaseUrl));
        
        return webhookUrls;
    }

    private string GetWebhookUrl(string baseUrl) {
        var webhookUrl = new Url(baseUrl.TrimEnd('/'));
        webhookUrl.AppendPathSegment($"umbraco/api/{WebhooksConstants.ApiName}/{CrowdfundingConstants.Webhooks.HookIds.Crowdfunder}");
        
        return webhookUrl.ToString();
    }
}