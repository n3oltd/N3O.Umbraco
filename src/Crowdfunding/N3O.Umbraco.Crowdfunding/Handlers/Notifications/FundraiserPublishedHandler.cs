using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Cloud.Engage;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
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

public class FundraiserPublishedHandler : IRequestHandler<FundraiserPublishedNotification, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly IContentService _contentService;
    private readonly ICrowdfunderManager _crowdfunderManager;
    private readonly ICoreScopeProvider _coreScopeProvider;
    private readonly DistributedCache _distributedCache;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FundraiserPublishedHandler(IContentLocator contentLocator,
                                      IContentService contentService,
                                      ICrowdfunderManager crowdfunderManager,
                                      ICoreScopeProvider coreScopeProvider,
                                      DistributedCache distributedCache,
                                      IWebHostEnvironment webHostEnvironment) {
        _contentLocator = contentLocator;
        _contentService = contentService;
        _crowdfunderManager = crowdfunderManager;
        _coreScopeProvider = coreScopeProvider;
        _distributedCache = distributedCache;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<None> Handle(FundraiserPublishedNotification req, CancellationToken cancellationToken) {
        IContent content;
        
        using (var scope = _coreScopeProvider.CreateCoreScope(autoComplete: true)) {
            using (_ = scope.Notifications.Suppress()) {
                content = req.ContentId.Run(id => _contentService.GetById(id), false);
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
        
        var fundraiser = _contentLocator.ById<FundraiserContent>(content.Key);
                
        if (!fundraiser.Status.HasValue()) {
            await _crowdfunderManager.CreateFundraiserAsync(fundraiser, GetWebhookUrls());
        } else {
            await _crowdfunderManager.UpdateCrowdfunderAsync(fundraiser.Key.ToString(),
                                                             fundraiser,
                                                             fundraiser.ToggleStatus,
                                                             GetWebhookUrls());
        }
        
        return None.Empty;
    }
    
    private IEnumerable<string> GetWebhookUrls() {
        var urlSettingsContent = _contentLocator.Single<UrlSettingsContent>();
        
        var webhookUrls = new List<string>();

        if (_webHostEnvironment.IsProduction()) {
            webhookUrls.Add(GetWebhookUrl(urlSettingsContent.ProductionBaseUrl));
        } else {
            webhookUrls.Add(GetWebhookUrl(urlSettingsContent.StagingBaseUrl));
        }
        
        return webhookUrls;
    }

    private string GetWebhookUrl(string baseUrl) {
        var webhookUrl = new Url(baseUrl.TrimEnd('/'));
        webhookUrl.AppendPathSegment($"umbraco/api/{WebhooksConstants.ApiName}/{CrowdfundingConstants.Webhooks.HookIds.Crowdfunder}");

        return webhookUrl.ToString();
    }
}