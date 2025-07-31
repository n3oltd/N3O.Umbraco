using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Robots;

public class RobotsContentPublishedNotification : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IRobotsFileService _robotsFileService;
    private readonly IContentTypeService _contentTypeService;
    
    public RobotsContentPublishedNotification(
        IRobotsFileService robotsFileService,
        IContentTypeService contentTypeService) {
        _robotsFileService = robotsFileService;
        _contentTypeService = contentTypeService;
    }
    
    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var entity in notification.PublishedEntities) {
            
            var contentType = _contentTypeService.Get(entity.ContentTypeId);
            
            if (contentType?.Alias == "robots") {
                await _robotsFileService.SaveRobotsFileToWwwroot();
                break;
            }
        }
    }
}