using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using N3O.Umbraco.Webhooks;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class FundraiserPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly ICrowdfunderManager _crowdfunderManager;
    private readonly IContentLocator _contentLocator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FundraiserPublished(ICrowdfunderManager crowdfunderManager,
                               IContentLocator contentLocator,
                               IWebHostEnvironment webHostEnvironment) {
        _crowdfunderManager = crowdfunderManager;
        _contentLocator = contentLocator;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias)) {
                var fundraiser = _contentLocator.ById<FundraiserContent>(content.Key);
                
                if (!fundraiser.Status.HasValue()) {
                    await _crowdfunderManager.CreateFundraiserAsync(fundraiser, GetWebhookUrl());
                } else {
                    await _crowdfunderManager.UpdateCrowdfunderAsync(fundraiser.Key.ToString(),
                                                                     fundraiser,
                                                                     fundraiser.ToggleStatus,
                                                                     GetWebhookUrl());
                }
            }
        }
    }
    
    private string GetWebhookUrl() {
        var urlSettingsContent = _contentLocator.Single<UrlSettingsContent>();
        
        var baseUrl = _webHostEnvironment.IsStaging()
                          ? urlSettingsContent.StagingBaseUrl
                          : urlSettingsContent.ProductionBaseUrl;

        var webhookUrl = new Url(baseUrl.TrimEnd('/'));
        webhookUrl.AppendPathSegment($"umbraco/api/{WebhooksConstants.ApiName}/{CrowdfundingConstants.Webhooks.HookIds.Crowdfunder}");

        return webhookUrl.ToString();
    }
}