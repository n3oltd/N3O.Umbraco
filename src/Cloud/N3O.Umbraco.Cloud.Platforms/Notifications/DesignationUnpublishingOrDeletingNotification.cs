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

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class DesignationUnpublishingOrDeletingNotification : INotificationAsyncHandler<ContentUnpublishingNotification>, 
                                                             INotificationAsyncHandler<ContentMovingToRecycleBinNotification> {
    private readonly IContentLocator _contentLocator;
    private readonly IContentTypeService _contentTypeService;

    public DesignationUnpublishingOrDeletingNotification(IContentLocator contentLocator,
                                                         IContentTypeService contentTypeService) {
        _contentLocator = contentLocator;
        _contentTypeService = contentTypeService;
    }

    public Task HandleAsync(ContentUnpublishingNotification notification, CancellationToken cancellationToken) {
        var designations = notification.UnpublishedEntities.Where(x => x.IsDesignation(_contentTypeService)).ToList();
        
        if (designations.HasAny()) {
            ValidateDefaultDesignationExists(designations, () => notification.CancelWithError("Cannot unpublish default designation"));
        }
        
        return Task.CompletedTask;
    }

    public Task HandleAsync(ContentMovingToRecycleBinNotification notification, CancellationToken cancellationToken) {
        var designations = notification.MoveInfoCollection
                                    .Where(x => x.Entity.IsDesignation(_contentTypeService))
                                    .Select(x => x.Entity)
                                    .ToList();
        
        if (designations.HasAny()) {
            ValidateDefaultDesignationExists(designations, () => notification.CancelWithError("Cannot delete default designation"));
        }
        
        return Task.CompletedTask;
    }

    private void ValidateDefaultDesignationExists(IEnumerable<IContent> designations, Action action) {
        foreach (var designationGroup in designations.GroupBy(x => x.ParentId)) {
            var contentKeys = designationGroup.Select(x => x.Key);
            
            var campaign = _contentLocator.ById(designationGroup.Key).As<CampaignContent>();
        
            var otherDesignations = campaign.Designations.ExceptWhere(x => contentKeys.Contains(x.Key));

            if (!otherDesignations.Any()) {
                action.Invoke();
            }
        }
    }
}