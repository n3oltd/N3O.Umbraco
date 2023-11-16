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
    INotificationAsyncHandler<ContentDeletedNotification>,
    INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IContentCache _contentCache;

    public FlushContentCacheHandlers(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public Task HandleAsync(ContentDeletedNotification notification, CancellationToken cancellationToken) {
        Process(notification.DeletedEntities);
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        Process(notification.PublishedEntities);
        
        return Task.CompletedTask;
    }

    private void Process(IEnumerable<IContent> entities) {
        var contentTypeAliases = entities.Select(x => x.ContentType.Alias).Distinct().ToList();

        if (contentTypeAliases.Any(x => _contentCache.ContainsContentType(x))) {
            _contentCache.Flush();
        }
    }
}
