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
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CampaignRemovingNotification :
    INotificationAsyncHandler<ContentUnpublishingNotification>,
    INotificationAsyncHandler<ContentMovingToRecycleBinNotification> {
    private readonly IContentLocator _contentLocator;
    private readonly IContentTypeService _contentTypeService;

    public CampaignRemovingNotification(IContentLocator contentLocator, IContentTypeService contentTypeService) {
        _contentLocator = contentLocator;
        _contentTypeService = contentTypeService;
    }

    public Task HandleAsync(ContentUnpublishingNotification notification, CancellationToken cancellationToken) {
        var campaignsBeingUnpublished = notification.UnpublishedEntities
                                                    .Where(x => x.IsCampaign(_contentTypeService))
                                                    .ToList();
        
        if (campaignsBeingUnpublished.Any() && !OtherCampaignsExist(campaignsBeingUnpublished)) {
            notification.CancelWithError("Cannot unpublish the default campaign");
        }
        
        return Task.CompletedTask;
    }

    public Task HandleAsync(ContentMovingToRecycleBinNotification notification, CancellationToken cancellationToken) {
        var campaignsBeingDeleted = notification.MoveInfoCollection
                                                .Where(x => x.Entity.IsCampaign(_contentTypeService))
                                                .Select(x => x.Entity)
                                                .ToList();
        
        if (campaignsBeingDeleted.Any() && !OtherCampaignsExist(campaignsBeingDeleted)) {
            notification.CancelWithError("Cannot delete the default campaign");
        }
        
        return Task.CompletedTask;
    }

    private bool OtherCampaignsExist(IEnumerable<IContent> campaignsBeingRemoved) {
        var beingRemovedKeys = campaignsBeingRemoved.Select(x => x.Key).ToList();
        
        var otherCampaigns = _contentLocator.All(x => x.IsComposedOf(AliasHelper<CampaignContent>.ContentTypeAlias()))
                                            .As<CampaignContent>()
                                            .ExceptWhere(x => beingRemovedKeys.Contains(x.Key))
                                            .ToList();

        return otherCampaigns.Any();
    }
}