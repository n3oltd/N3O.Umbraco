using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public abstract class CloudContentNotification {
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly IBackgroundJob _backgroundJob;

    protected CloudContentNotification(ISubscriptionAccessor subscriptionAccessor, IBackgroundJob backgroundJob) {
        _subscriptionAccessor = subscriptionAccessor;
        _backgroundJob = backgroundJob;
    }
    
    protected void Enqueue(object body) {
        var subscription = _subscriptionAccessor.GetSubscription();
                
        var req = new DispatchWebhookReq();
        req.Body = body;
        req.Url = $"https://n3o.cloud/eu1/hooks/{HookId}/{subscription.Id.Number}";
                
        _backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req);
    }

    protected abstract string HookId { get; }
}