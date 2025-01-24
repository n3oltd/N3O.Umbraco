using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Events;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Extensions;
using N3O.Umbraco.Webhooks.Models;
using N3O.Umbraco.Webhooks.Receivers;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Webhooks;

[WebhookReceiver(CrowdfundingConstants.Webhooks.HookIds.Crowdfunder)]
public class CrowdfunderReceiver : WebhookReceiver {
    private readonly IBackgroundJob _backgroundJob;
    private readonly IJsonProvider _jsonProvider;
    private readonly AsyncKeyedLocker<string> _locker;

    public CrowdfunderReceiver(IJsonProvider jsonProvider,
                               AsyncKeyedLocker<string> locker,
                               IBackgroundJob backgroundJob) {
        _jsonProvider = jsonProvider;
        _locker = locker;
        _backgroundJob = backgroundJob;
    }
    
    protected override async Task ProcessAsync(WebhookPayload payload, CancellationToken cancellationToken) {
        var contentId = payload.GetHeader(CrowdfundingConstants.Webhooks.Headers.CrowdfunderId);
        var webhookCrowdfunder = payload.GetBody<JobResult>(_jsonProvider);

        if (!contentId.HasValue() && !webhookCrowdfunder.CrowdfunderInfo.HasValue()) {
            return;
        }
        
        using (await _locker.LockAsync(contentId, cancellationToken)) {
            var eventType = payload.GetHeader(CrowdfundingConstants.Webhooks.Headers.JobType);

            switch (eventType) {
                case CrowdfundingConstants.Webhooks.EventTypes.Crowdfunder.CampaignCreated:
                case CrowdfundingConstants.Webhooks.EventTypes.Crowdfunder.FundraiserCreated:
                    Enqueue<CrowdfunderCreatedEvent>(contentId, webhookCrowdfunder);
                    break;
                
                case CrowdfundingConstants.Webhooks.EventTypes.Crowdfunder.CrowdfunderSynced:
                    Enqueue<CrowdfunderUpdatedEvent>(contentId, webhookCrowdfunder);
                    break;
            }
        }
    }

    private void Enqueue<TEvent>(string contentId, JobResult webhook) where TEvent : CrowdfunderEvent {
        _backgroundJob.Enqueue<TEvent, JobResult>($"{typeof(TEvent).Name.Replace("Event", "")}",
                                                             webhook,
                                                             p => p.Add<ContentId>(contentId));
    }
}