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

public class ElementUnpublishingOrDeletingNotification : INotificationAsyncHandler<ContentUnpublishingNotification>,
                                                         INotificationAsyncHandler<ContentMovingToRecycleBinNotification> {
    private static readonly string ElementCampaignAlias = AliasHelper<IPlatformsElement>.PropertyAlias(x => x.Campaign);
    
    private readonly IContentLocator _contentLocator;
    private readonly IContentTypeService _contentTypeService;

    public ElementUnpublishingOrDeletingNotification(IContentLocator contentLocator,
                                                         IContentTypeService contentTypeService) {
        _contentLocator = contentLocator;
        _contentTypeService = contentTypeService;
    }

    public Task HandleAsync(ContentUnpublishingNotification notification, CancellationToken cancellationToken) {
        var elements = notification.UnpublishedEntities.Where(x => x.IsElement(_contentTypeService)).ToList();
        
        if (elements.HasAny()) {
            ValidateDefaultElementExists(elements, () => notification.CancelWithError("Cannot unpublish default element"));
        }
        
        return Task.CompletedTask;
    }

    public Task HandleAsync(ContentMovingToRecycleBinNotification notification, CancellationToken cancellationToken) {
        var elements = notification.MoveInfoCollection
                                   .Where(x => x.Entity.IsElement(_contentTypeService))
                                   .Select(x => x.Entity)
                                   .ToList();
        
        if (elements.HasAny()) {
            ValidateDefaultElementExists(elements, () => notification.CancelWithError("Cannot delete default element"));
        }
        
        return Task.CompletedTask;
    }

    private void ValidateDefaultElementExists(IEnumerable<IContent> elements, Action action) {
        var elementsRoot = _contentLocator.Single<PlatformsElements>();
        var elementsGroup = elements.GroupBy(x => x.GetValue(ElementCampaignAlias));
        
        foreach (var elementGroup in elementsGroup) {
            var keys = elementGroup.Select(x => x.Key).ToList();
            
            var otherElements = elementsRoot.DescendantsOfType(elementGroup.First().ContentType.Alias)
                                            .ExceptWhere(x => keys.Contains(x.Key));
            
            if (!otherElements.Any()) {
                action.Invoke();
                
                return;
            }
        }
    }
}