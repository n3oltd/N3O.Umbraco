using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public abstract class PlatformsContentNotification {
    private readonly ICloudUrl _cloudUrl;
    private readonly IBackgroundJob _backgroundJob;

    protected PlatformsContentNotification(ICloudUrl cloudUrl, IBackgroundJob backgroundJob) {
        _cloudUrl = cloudUrl;
        _backgroundJob = backgroundJob;
    }
    
    protected void Enqueue(object body) {
        var req = new DispatchWebhookReq();
        req.Body = body;
        req.Url = _cloudUrl.ForWebhook(HookId);
        
        _backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req, HookId);
    }

    protected abstract string HookId { get; }
}