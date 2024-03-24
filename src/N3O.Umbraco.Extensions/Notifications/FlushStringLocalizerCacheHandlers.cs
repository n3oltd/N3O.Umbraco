using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

public class FlushStringLocalizerCacheHandlers :
    INotificationAsyncHandler<ContentMovedToRecycleBinNotification>,
    INotificationAsyncHandler<ContentPublishedNotification>,
    INotificationAsyncHandler<ContentUnpublishedNotification> {
    private readonly IStringLocalizer _stringLocalizer;

    public FlushStringLocalizerCacheHandlers(IStringLocalizer stringLocalizer) {
        _stringLocalizer = stringLocalizer;
    }

    public async Task HandleAsync(ContentMovedToRecycleBinNotification notification,
                                  CancellationToken cancellationToken) {
        await ProcessAsync(notification.MoveInfoCollection.Select(x => x.Entity));
    }
    
    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        await ProcessAsync(notification.PublishedEntities);
    }

    public async Task HandleAsync(ContentUnpublishedNotification notification, CancellationToken cancellationToken) {
        await ProcessAsync(notification.UnpublishedEntities);
    }

    private Task ProcessAsync(IEnumerable<IContent> entities) {
        var aliases = entities.OrEmpty().Select(x => x.ContentType.Alias).Distinct().ToList();
    
        _stringLocalizer.Flush(aliases);
    
        return Task.CompletedTask;
    }
}
