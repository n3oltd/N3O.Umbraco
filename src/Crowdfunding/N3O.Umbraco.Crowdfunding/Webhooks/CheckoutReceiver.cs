using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Events;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Extensions;
using N3O.Umbraco.Webhooks.Models;
using N3O.Umbraco.Webhooks.Receivers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Webhooks;

[WebhookReceiver(CrowdfundingConstants.Webhooks.HookIds.Checkout)]
public class CheckoutReceiver : WebhookReceiver {
    private readonly IBackgroundJob _backgroundJob;
    private readonly IJsonProvider _jsonProvider;
    private readonly AsyncKeyedLocker<string> _locker;

    public CheckoutReceiver(IJsonProvider jsonProvider, AsyncKeyedLocker<string> locker, IBackgroundJob backgroundJob) {
        _jsonProvider = jsonProvider;
        _locker = locker;
        _backgroundJob = backgroundJob;
    }
    
    protected override async Task ProcessAsync(WebhookPayload payload, CancellationToken cancellationToken) {
        var webhookCheckout = payload.GetBody<WebhookCheckout>(_jsonProvider);

        if (!webhookCheckout.Donation.CartItems.Any(x => x.HasCrowdfunderData()) || !webhookCheckout.Donation.IsComplete) {
            return;
        }
        
        using (await _locker.LockAsync(webhookCheckout.Revision.Id.ToString(), cancellationToken)) {
            var eventType = payload.GetEventType();

            switch (eventType) {
                case CrowdfundingConstants.Webhooks.EventTypes.Checkout.CheckoutUpdated:
                    Enqueue<CheckoutUpdatedEvent>(webhookCheckout);
                    break;
            }
        }
    }

    private void Enqueue<TEvent>(WebhookCheckout webhookCheckout) where TEvent : CheckoutEvent {
        _backgroundJob.Enqueue<TEvent, WebhookCheckout>($"{typeof(TEvent).Name.Replace("Event", "")}",
                                                        webhookCheckout);
    }
}