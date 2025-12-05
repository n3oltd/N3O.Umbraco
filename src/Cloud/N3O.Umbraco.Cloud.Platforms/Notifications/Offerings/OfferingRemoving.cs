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

public class OfferingRemoving :
    INotificationAsyncHandler<ContentUnpublishingNotification>,
    INotificationAsyncHandler<ContentMovingToRecycleBinNotification> {
    private readonly IContentLocator _contentLocator;
    private readonly IContentTypeService _contentTypeService;

    public OfferingRemoving(IContentLocator contentLocator, IContentTypeService contentTypeService) {
        _contentLocator = contentLocator;
        _contentTypeService = contentTypeService;
    }

    public Task HandleAsync(ContentUnpublishingNotification notification, CancellationToken cancellationToken) {
        var offeringsBeingUnpublished = notification.UnpublishedEntities
                                                       .Where(x => x.IsOffering(_contentTypeService))
                                                       .ToList();
        
        if (offeringsBeingUnpublished.Any() && !OtherOfferingsExist(offeringsBeingUnpublished)) {
            notification.CancelWithError("Cannot unpublish the default offering");
        }
        
        return Task.CompletedTask;
    }

    public Task HandleAsync(ContentMovingToRecycleBinNotification notification, CancellationToken cancellationToken) {
        var offeringsBeingDeleted = notification.MoveInfoCollection
                                                   .Where(x => x.Entity.IsOffering(_contentTypeService))
                                                   .Select(x => x.Entity)
                                                   .ToList();
        
        if (offeringsBeingDeleted.Any() && !OtherOfferingsExist(offeringsBeingDeleted)) {
            notification.CancelWithError("Cannot delete the default offering");
        }
        
        return Task.CompletedTask;
    }

    private bool OtherOfferingsExist(IEnumerable<IContent> offerings) {
        foreach (var perCampaignGroup in offerings.GroupBy(x => x.ParentId)) {
            var campaign = _contentLocator.ById(perCampaignGroup.Key).As<CampaignContent>();
            var beingRemovedKeys = perCampaignGroup.Select(x => x.Key).ToList();
        
            var otherOfferings = campaign.Offerings.ExceptWhere(x => beingRemovedKeys.Contains(x.Key));

            if (otherOfferings.None()) {
                return false;
            }
        }

        return true;
    }
}