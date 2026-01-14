using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Content.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class FeedPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentLocator> _contentLocator;

    public FeedPublished(ISubscriptionAccessor subscriptionAccessor,
                         ICloudUrl cloudUrl,
                         IBackgroundJob backgroundJob,
                         IContentTypeService contentTypeService,
                         Lazy<IContentLocator> contentLocator,
                         ILogger<FeedPublished> logger)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
    }

    protected override object GetBody(IContent content) {
        var feed = _contentLocator.Value.ById(content.Id);
        
        var req = new ContentCollectionWebhookBodyReq();
        req.Id = content.Key.ToString();
        req.ContentLibraryId = feed.Ancestors().Single(x => x.ContentType.Alias == PlatformsConstants.Feeds.Alias).Key.ToString();
        req.Action = WebhookSyncAction.AddOrUpdate;
        req.AddOrUpdate = new ContentCollectionReq();
        req.AddOrUpdate.Name = content.Name;

        return req;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsFeed(_contentTypeService);
    }

    protected override string HookId => PlatformsConstants.WebhookIds.ContentCollection;
}