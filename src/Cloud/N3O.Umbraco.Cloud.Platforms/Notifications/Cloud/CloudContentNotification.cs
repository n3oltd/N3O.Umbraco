using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public abstract class CloudContentNotification {
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly ICloudUrl _cloudUrl;
    private readonly IBackgroundJob _backgroundJob;

    protected CloudContentNotification(ISubscriptionAccessor subscriptionAccessor,
                                       ICloudUrl cloudUrl,
                                       IBackgroundJob backgroundJob) {
        _subscriptionAccessor = subscriptionAccessor;
        _cloudUrl = cloudUrl;
        _backgroundJob = backgroundJob;
    }
    
    protected void Enqueue(object body) {
        var subscription = _subscriptionAccessor.GetSubscription();
                
        var req = new DispatchWebhookReq();
        req.Body = body;
        req.Url = _cloudUrl.ForWebhook(HookId);
                
        //_backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req); TODO revert once the platforms work is done
    }

    protected abstract string HookId { get; }
}