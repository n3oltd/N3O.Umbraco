using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CampaignUnpublishingOrDeletingNotification : INotificationAsyncHandler<ContentUnpublishingNotification>, 
                                                          INotificationAsyncHandler<ContentMovingToRecycleBinNotification> {
    private readonly IContentLocator _contentLocator;
    private readonly IContentTypeService _contentTypeService;

    public CampaignUnpublishingOrDeletingNotification(IContentLocator contentLocator,
                                                      IContentTypeService contentTypeService) {
        _contentLocator = contentLocator;
        _contentTypeService = contentTypeService;
    }

    public Task HandleAsync(ContentUnpublishingNotification notification, CancellationToken cancellationToken) {
        var campaigns = notification.UnpublishedEntities.Where(x => x.IsCampaign(_contentTypeService)).ToList();
        
        if (campaigns.HasAny()) {
            ValidateDefaultCampaignExists(campaigns, () => notification.CancelWithError("Cannot unpublish default campaign"));
        }
        
        return Task.CompletedTask;
    }

    public Task HandleAsync(ContentMovingToRecycleBinNotification notification, CancellationToken cancellationToken) {
        var campaigns = notification.MoveInfoCollection
                                    .Where(x => x.Entity.IsCampaign(_contentTypeService))
                                    .Select(x => x.Entity)
                                    .ToList();
        
        if (campaigns.HasAny()) {
            ValidateDefaultCampaignExists(campaigns, () => notification.CancelWithError("Cannot delete default campaign"));
        }
        
        return Task.CompletedTask;
    }

    private void ValidateDefaultCampaignExists(IEnumerable<IContent> contents, Action action) {
        var contentKeys = contents.Select(x => x.Key);
        
        var campaigns = _contentLocator.All(x => x.IsComposedOf(AliasHelper<CampaignContent>.ContentTypeAlias()))
                                       .As<CampaignContent>()
                                       .ExceptWhere(x => contentKeys.Contains(x.Key));

        if (!campaigns.Any()) {
            action.Invoke();
        }
    }
}