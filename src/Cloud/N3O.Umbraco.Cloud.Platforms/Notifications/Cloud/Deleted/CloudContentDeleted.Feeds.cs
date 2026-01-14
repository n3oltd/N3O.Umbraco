using N3O.Umbraco.Cloud.Content.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class FeedsDeleted : CloudContentDeleted {
    public FeedsDeleted(ISubscriptionAccessor subscriptionAccessor,
                        ICloudUrl cloudUrl,
                        IBackgroundJob backgroundJob)
        : base(subscriptionAccessor, cloudUrl, backgroundJob) {
    }

    protected override object GetBody(IContent content) {
        var req = new ContentLibraryWebhookBodyReq();
        req.Id = content.Key.ToString();
        req.Action = WebhookSyncAction.Delete;

        return req;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsFeeds();
    }

    protected override string HookId => PlatformsConstants.WebhookIds.ContentLibrary;
}