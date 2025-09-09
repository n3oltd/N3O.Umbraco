using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ElementRemoving :
    INotificationAsyncHandler<ContentUnpublishingNotification>,
    INotificationAsyncHandler<ContentMovingToRecycleBinNotification> {
    private readonly IContentTypeService _contentTypeService;
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

    public ElementRemoving(IContentTypeService contentTypeService,
                           IBackOfficeSecurityAccessor backOfficeSecurityAccessor) {
        _contentTypeService = contentTypeService;
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
    }

    public Task HandleAsync(ContentUnpublishingNotification notification, CancellationToken cancellationToken) {
        var elementsBeingUnpublished = notification.UnpublishedEntities
                                                    .Where(x => x.IsElement(_contentTypeService))
                                                    .ToList();
        
        if (elementsBeingUnpublished.Any(IsDefaultElement)) {
            if (!IsAdmin()) {
                notification.CancelWithError("Cannot unpublish the default element");
            }
        }
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentMovingToRecycleBinNotification notification, CancellationToken cancellationToken) {
        var elementsBeingDeleted = notification.MoveInfoCollection
                                               .Where(x => x.Entity.IsElement(_contentTypeService))
                                               .Select(x => x.Entity)
                                               .ToList();
        
        if (elementsBeingDeleted.Any(IsDefaultElement)) {
            if (!IsAdmin()) {
                notification.CancelWithError("Cannot delete the default element");
            }
        }
        
        return Task.CompletedTask;
    }

    private bool IsAdmin() {
        var user = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;
        
        return user?.IsAdmin() == true;
    }
    
    private bool IsDefaultElement(IContent content) {
        return content.GetValue<bool>(AliasHelper<ElementContent>.PropertyAlias(x => x.IsSystemGenerated));
    }
}