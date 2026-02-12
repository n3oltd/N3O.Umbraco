using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class OfferingMoving : INotificationAsyncHandler<ContentMovingNotification> {
    private readonly IContentTypeService _contentTypeService;
    
    public OfferingMoving(IContentTypeService contentTypeService) {
        _contentTypeService = contentTypeService;
    }

    public Task HandleAsync(ContentMovingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.MoveInfoCollection.Select(x => x.Entity)) {
            if (content.IsOffering(_contentTypeService)) {
                notification.CancelWithError("An offering cannot be moved, please deactivate and copy instead");
            }
        }
        
        return Task.CompletedTask;
    }
}
