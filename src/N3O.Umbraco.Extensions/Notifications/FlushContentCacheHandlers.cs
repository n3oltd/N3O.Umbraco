using N3O.Umbraco.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

public class FlushContentCacheHandlers :
    INotificationAsyncHandler<ContentMovedToRecycleBinNotification>,
    INotificationAsyncHandler<ContentPublishedNotification>,
    INotificationAsyncHandler<ContentUnpublishedNotification> {
    private readonly IContentCache _contentCache;

    public FlushContentCacheHandlers(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public Task HandleAsync(ContentMovedToRecycleBinNotification notification, CancellationToken cancellationToken) {
        Process(notification.MoveInfoCollection.Select(x => x.Entity));
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        Process(notification.PublishedEntities);
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentUnpublishedNotification notification, CancellationToken cancellationToken) {
        Process(notification.UnpublishedEntities);
        
        return Task.CompletedTask;
    }


    private void Process(IEnumerable<IContent> entities) {
        foreach (var contentTypeAlias in entities.Select(x => x.ContentType.Alias).Distinct()) {
            _contentCache.Flush(contentTypeAlias);
        }
    }
}
