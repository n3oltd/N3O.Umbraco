using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Extensions;
using N3O.Umbraco.Webhooks.Models;
using N3O.Umbraco.Webhooks.Receivers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static N3O.Umbraco.Cloud.Platforms.PlatformsConstants.Webhooks;

namespace N3O.Umbraco.Cloud.Platforms.Webhooks;

[WebhookReceiver(HookIds.Crowdfunder)]
public class CrowdfunderReceiver : WebhookReceiver {
    private readonly ICdnClient _cdnClient;
    private readonly IJsonProvider _jsonProvider;

    public CrowdfunderReceiver(ICdnClient cdnClient, IJsonProvider jsonProvider) {
        _cdnClient = cdnClient;
        _jsonProvider = jsonProvider;
    }

    protected override async Task ProcessAsync(WebhookPayload payload, CancellationToken cancellationToken) {
        var webhookCrowdfunder = payload.GetBody<WebhookCrowdfunder>(_jsonProvider);
        var eventType = payload.GetEventType();

        switch (eventType) {
            case EventTypes.Crowdfunder.Created:
            case EventTypes.Crowdfunder.Updated:
                await EvictAsync(webhookCrowdfunder, cancellationToken);

                break;
        }
    }

    private async Task EvictAsync(WebhookCrowdfunder webhookCrowdfunder, CancellationToken cancellationToken) {
        if (webhookCrowdfunder == null || !webhookCrowdfunder.HasValue(x => x.PagePublishedPath)) {
            return;
        }

        foreach (var pagePublishedPath in webhookCrowdfunder.OrEmpty(x => x.PagePublishedPathsHistory)) {
            _cdnClient.Evict(pagePublishedPath);
        }

        _cdnClient.Evict(webhookCrowdfunder.PagePublishedPath);

        await EvictMergeModelsAsync(webhookCrowdfunder.PagePublishedPath, cancellationToken);
    }

    private async Task EvictMergeModelsAsync(string pagePublishedPath, CancellationToken cancellationToken) {
        var publishedContentResult = await _cdnClient.DownloadPublishedContentAsync(pagePublishedPath,
                                                                                   cancellationToken);

        if (publishedContentResult.NotFound || publishedContentResult.Error) {
            return;
        }

        var content = publishedContentResult.Content;
        var publishedPlatformsPage = _jsonProvider.DeserializeDynamicTo<PublishedPlatformsPage>(content);

        foreach (var mergeModel in publishedPlatformsPage.OrEmpty(x => x.MergeModels)) {
            _cdnClient.Evict(mergeModel.Path);
        }
    }

    public class WebhookCrowdfunder {
        public WebhookCrowdfunder(string pagePublishedPath, IEnumerable<string> pagePublishedPathsHistory) {
            PagePublishedPath = pagePublishedPath;
            PagePublishedPathsHistory = pagePublishedPathsHistory;
        }

        public string PagePublishedPath { get; }
        public IEnumerable<string> PagePublishedPathsHistory { get; }
    }
}
