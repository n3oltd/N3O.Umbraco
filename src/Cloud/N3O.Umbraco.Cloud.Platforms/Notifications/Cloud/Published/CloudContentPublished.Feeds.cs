using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Content.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class FeedsPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;

    public FeedsPublished(ISubscriptionAccessor subscriptionAccessor,
                          ICloudUrl cloudUrl,
                          IBackgroundJob backgroundJob,
                          IContentTypeService contentTypeService,
                          ILogger<FeedsPublished> logger)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentTypeService = contentTypeService;
    }

    protected override object GetBody(IContent content) {
        var req = new ContentLibraryWebhookBodyReq();
        req.Id = content.Key.ToString();
        req.Action = WebhookSyncAction.AddOrUpdate;
        req.AddOrUpdate = new ContentLibraryReq();
        req.AddOrUpdate.Name = content.Name;

        return req;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsFeeds();
    }

    protected override string HookId => PlatformsConstants.WebhookIds.ContentLibrary;
}