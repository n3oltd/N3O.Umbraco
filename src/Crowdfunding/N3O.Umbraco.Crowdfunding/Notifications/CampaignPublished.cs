using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Utilities;
using N3O.Umbraco.Webhooks;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CampaignPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly ICrowdfunderManager _crowdfunderManager;
    private readonly IContentLocator _contentLocator;
    private readonly IBackgroundJob _backgroundJob;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;

    public CampaignPublished(ICrowdfunderManager crowdfunderManager,
                             IContentLocator contentLocator,
                             IBackgroundJob backgroundJob,
                             IWebHostEnvironment webHostEnvironment,
                             ICrowdfundingUrlBuilder crowdfundingUrlBuilder) {
        _crowdfunderManager = crowdfunderManager;
        _backgroundJob = backgroundJob;
        _webHostEnvironment = webHostEnvironment;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
        _contentLocator = contentLocator;
    }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias)) {
                var campaign = _contentLocator.ById<CampaignContent>(content.Key);
                var urlSettingsContent = _contentLocator.Single<UrlSettingsContent>();

                if (!campaign.Status.HasValue()) {
                    await _crowdfunderManager.CreateCampaignAsync(campaign, GetWebhookUrl(urlSettingsContent));
                } else {
                    await _crowdfunderManager.UpdateCrowdfunderAsync(campaign.Key.ToString(),
                                                                     campaign,
                                                                     campaign.ToggleStatus,
                                                                     GetWebhookUrl(urlSettingsContent));
                }

                if (_webHostEnvironment.IsProduction() &&
                    campaign.Status.HasValue() &&
                    campaign.Status != CrowdfunderStatuses.Draft) {
                    EnqueueCampaignWebhook(campaign, urlSettingsContent);
                }
            }
        }
    }

    private string GetWebhookUrl(UrlSettingsContent urlSettingsContent) {
        var baseUrl = _webHostEnvironment.IsStaging() ? urlSettingsContent.StagingBaseUrl : urlSettingsContent.ProductionBaseUrl;
        
        var webhookUrl = new Url(baseUrl.TrimEnd('/'));
        webhookUrl.AppendPathSegment($"umbraco/api/{WebhooksConstants.ApiName}/{CrowdfundingConstants.Webhooks.HookIds.Crowdfunder}");
        
        return webhookUrl.ToString();
    }

    private void EnqueueCampaignWebhook(CampaignContent campaign, UrlSettingsContent urlSettingsContent) {
        var stagingBaseUrl = urlSettingsContent.StagingBaseUrl;
        var campaignUrl = campaign.Url(_crowdfundingUrlBuilder);

        _backgroundJob.EnqueueCampaignUrlWebhook(campaign.Key, campaignUrl, stagingBaseUrl);
    }
}