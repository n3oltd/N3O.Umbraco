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
        var webhookCrowdfunder = payload.GetBody<WebhookCrowdfunderInfo>(_jsonProvider);

        if (!webhookCrowdfunder.HasValue()) {
            return;
        }
        
        using (await _locker.LockAsync(webhookCrowdfunder.Id.ToString(), cancellationToken)) {
            var eventType = payload.GetEventType();

            switch (eventType) {
                case CrowdfundingConstants.Webhooks.EventTypes.Crowdfunder.CrowdfunderCreated:
                    Enqueue<CrowdfunderCreatedEvent>(webhookCrowdfunder);
                    break;
                
                case CrowdfundingConstants.Webhooks.EventTypes.Crowdfunder.CrowdfunderUpdated:
                    Enqueue<CrowdfunderUpdatedEvent>(webhookCrowdfunder);
                    break;
            }
        }
    }

    private void Enqueue<TEvent>(WebhookCrowdfunderInfo webhookCrowdfunder) where TEvent : CrowdfunderEvent {
        var contentId = webhookCrowdfunder.Id;
        
        _backgroundJob.Enqueue<TEvent, WebhookCrowdfunderInfo>($"{typeof(TEvent).Name.Replace("Event", "")}",
                                                               webhookCrowdfunder,
                                                               p => p.Add<ContentId>(contentId.ToString()));
    }
}