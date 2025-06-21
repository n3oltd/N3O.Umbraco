using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class PlatformsContentChangedOrRemovedNotification :
    INotificationAsyncHandler<ContentPublishedNotification>,
    INotificationAsyncHandler<ContentUnpublishedNotification>,
    INotificationAsyncHandler<ContentMovedToRecycleBinNotification> {
    private readonly Lazy<IBackgroundJob> _backgroundJob;
    private readonly IContentCache _contentCache;

    public PlatformsContentChangedOrRemovedNotification(Lazy<IBackgroundJob> backgroundJob, IContentCache contentCache) {
        _backgroundJob = backgroundJob;
        _contentCache = contentCache;
    }
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        if (notification.PublishedEntities.Any(x => x.IsPlatformsContent(_contentCache))) {
            _backgroundJob.Value.EnqueueCommand<PublishPlatformsContentCommand>();
        }
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentUnpublishedNotification notification, CancellationToken cancellationToken) {
        if (notification.UnpublishedEntities.Any(x => x.IsPlatformsContent(_contentCache))) {
            _backgroundJob.Value.EnqueueCommand<PublishPlatformsContentCommand>();
        }
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentMovedToRecycleBinNotification notification, CancellationToken cancellationToken) {
        if (notification.MoveInfoCollection.Select(x => x.Entity).Any(x => x.IsPlatformsContent(_contentCache))) {
            _backgroundJob.Value.EnqueueCommand<PublishPlatformsContentCommand>();
        }
        
        return Task.CompletedTask;
    }
}