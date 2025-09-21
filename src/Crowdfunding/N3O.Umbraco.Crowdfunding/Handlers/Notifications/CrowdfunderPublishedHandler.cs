using Flurl;
using N3O.Umbraco.Cloud.Engage;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Utilities;
using N3O.Umbraco.Webhooks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Changes;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class CrowdfunderPublishedHandler : IRequestHandler<CrowdfunderPublishedNotification, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly IContentService _contentService;
    private readonly ICrowdfunderManager _crowdfunderManager;
    private readonly ICoreScopeProvider _coreScopeProvider;
    private readonly DistributedCache _distributedCache;

    public CrowdfunderPublishedHandler(IContentLocator contentLocator,
                                     IContentService contentService,
                                     ICrowdfunderManager crowdfunderManager,
                                     ICoreScopeProvider coreScopeProvider,
                                     DistributedCache distributedCache) {
        _contentLocator = contentLocator;
        _contentService = contentService;
        _crowdfunderManager = crowdfunderManager;
        _coreScopeProvider = coreScopeProvider;
        _distributedCache = distributedCache;
    }

    public async Task<None> Handle(CrowdfunderPublishedNotification req, CancellationToken cancellationToken) {
        IContent content;
        
        using (var scope = _coreScopeProvider.CreateCoreScope(autoComplete: true)) {
            using (_ = scope.Notifications.Suppress()) {
                content = _contentService.GetById(req.ContentId.Value);
            }
        }

        if (content == null) {
            return None.Empty;
        }
        
        var payload = new ContentCacheRefresher.JsonPayload {
            Id = content.Id,
            Key = content.Key,
            ChangeTypes = TreeChangeTypes.RefreshNode,
            Blueprint = content.Blueprint,
            PublishedCultures = content.PublishedCultures?.ToArray(),
            UnpublishedCultures = content.AvailableCultures?.Except(content.PublishedCultures)?.ToArray()
        };

        _distributedCache.RefreshByPayload(ContentCacheRefresher.UniqueId, payload.Yield());
        
        var campaign = _contentLocator.ById<CampaignContent>(req.ContentId.Value);
        var urlSettingsContent = _contentLocator.Single<UrlSettingsContent>();

        if (!campaign.Status.HasValue()) {
            await _crowdfunderManager.CreateCampaignAsync(campaign, GetWebhookUrls(urlSettingsContent));
        } else {
            await _crowdfunderManager.UpdateCrowdfunderAsync(campaign.Key.ToString(),
                                                             campaign,
                                                             campaign.ToggleStatus,
                                                             GetWebhookUrls(urlSettingsContent));
        }
        
        return None.Empty;
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