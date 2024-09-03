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

[WebhookReceiver(CrowdfundingConstants.Webhooks.HookIds.Pledges)]
public class PledgesReceiver : WebhookReceiver {
    private readonly IBackgroundJob _backgroundJob;
    private readonly IJsonProvider _jsonProvider;
    private readonly AsyncKeyedLocker<string> _locker;

    public PledgesReceiver(IJsonProvider jsonProvider, AsyncKeyedLocker<string> locker, IBackgroundJob backgroundJob) {
        _jsonProvider = jsonProvider;
        _locker = locker;
        _backgroundJob = backgroundJob;
    }
    
    protected override async Task ProcessAsync(WebhookPayload payload, CancellationToken cancellationToken) {
        var webhookPledge = payload.GetBody<WebhookPledge>(_jsonProvider);

        if (!webhookPledge.Crowdfunding.HasValue()) {
            return;
        }
        
        using (await _locker.LockAsync(webhookPledge.Revision.Id.ToString(), cancellationToken)) {
            var eventType = payload.GetEventType();

            switch (eventType) {
                case CrowdfundingConstants.Webhooks.EventTypes.Pledges.PledgeUpdated:
                    Enqueue<PledgeUpdatedEvent>(webhookPledge);
                    break;
            }
        }
    }

    private void Enqueue<TEvent>(WebhookPledge webhookPledge) where TEvent : PledgeEvent {
        var contentId = webhookPledge.Crowdfunding.FundraiserId ?? webhookPledge.Crowdfunding.CampaignId;
        
        _backgroundJob.Enqueue<TEvent, WebhookPledge>($"{typeof(TEvent).Name.Replace("Event", "")}",
                                                      webhookPledge,
                                                      p => p.Add<ContentId>(contentId.ToString()));
    }
}