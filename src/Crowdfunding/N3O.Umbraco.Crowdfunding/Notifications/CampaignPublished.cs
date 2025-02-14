﻿using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Utilities;
using N3O.Umbraco.Webhooks;
using System.Collections.Generic;
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
        if (_webHostEnvironment.IsProduction()) {
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