using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Content.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class FeedItemPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public FeedItemPublished(ISubscriptionAccessor subscriptionAccessor,
                             ICloudUrl cloudUrl,
                             IBackgroundJob backgroundJob,
                             IContentTypeService contentTypeService,
                             Lazy<IContentLocator> contentLocator,
                             IUmbracoMapper mapper,
                             ILogger<FeedItemPublished> logger)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
        _mapper = mapper;
    }

    protected override object GetBody(IContent content) {
        var feedItem = _contentLocator.Value.ById<IPublishedContent>(content.Key);

        var req = _mapper.Map<IPublishedContent, ManagedContentWebhookBodyReq>(feedItem);

        return req;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsFeedItem(_contentTypeService) && IsApprovedOrRejectedFolder(content);
    }

    private bool IsApprovedOrRejectedFolder(IContent content) {
        var parent = _contentLocator.Value.ById<IPublishedContent>(content.ParentId);

        if (parent.ContentType.Alias.IsAnyOf(PlatformsConstants.FeedsItemFolders.ApprovedFolder,
                                             PlatformsConstants.FeedsItemFolders.RejectedFolder)) {
            return true;
        }
        
        return false;
    }

    protected override string HookId => PlatformsConstants.WebhookIds.ManagedContent;
}