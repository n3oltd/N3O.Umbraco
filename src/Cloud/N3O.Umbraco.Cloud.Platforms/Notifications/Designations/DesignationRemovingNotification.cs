using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class DesignationRemovingNotification :
    INotificationAsyncHandler<ContentUnpublishingNotification>,
    INotificationAsyncHandler<ContentMovingToRecycleBinNotification> {
    private readonly IContentLocator _contentLocator;
    private readonly IContentTypeService _contentTypeService;

    public DesignationRemovingNotification(IContentLocator contentLocator, IContentTypeService contentTypeService) {
        _contentLocator = contentLocator;
        _contentTypeService = contentTypeService;
    }

    public Task HandleAsync(ContentUnpublishingNotification notification, CancellationToken cancellationToken) {
        var designationsBeingUnpublished = notification.UnpublishedEntities
                                                    .Where(x => x.IsDesignation(_contentTypeService))
                                                    .ToList();
        
        if (designationsBeingUnpublished.Any() && !OtherDesignationsExist(designationsBeingUnpublished)) {
            notification.CancelWithError("Cannot unpublish the default designation");
        }
        
        return Task.CompletedTask;
    }

    public Task HandleAsync(ContentMovingToRecycleBinNotification notification, CancellationToken cancellationToken) {
        var designationsBeingDeleted = notification.MoveInfoCollection
                                                   .Where(x => x.Entity.IsDesignation(_contentTypeService))
                                                   .Select(x => x.Entity)
                                                   .ToList();
        
        if (designationsBeingDeleted.Any() && !OtherDesignationsExist(designationsBeingDeleted)) {
            notification.CancelWithError("Cannot delete the default designation");
        }
        
        return Task.CompletedTask;
    }

    private bool OtherDesignationsExist(IEnumerable<IContent> designations) {
        foreach (var perCampaignGroup in designations.GroupBy(x => x.ParentId)) {
            var campaign = _contentLocator.ById(perCampaignGroup.Key).As<CampaignContent>();
            var beingRemovedKeys = perCampaignGroup.Select(x => x.Key).ToList();
        
            var otherDesignations = campaign.Designations.ExceptWhere(x => beingRemovedKeys.Contains(x.Key));

            if (otherDesignations.None()) {
                return false;
            }
        }

        return true;
    }
}