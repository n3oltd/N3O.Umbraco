using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class PlatformsSettingsChangedOrRemovedNotification :
    INotificationAsyncHandler<ContentPublishedNotification>,
    INotificationAsyncHandler<ContentUnpublishedNotification>,
    INotificationAsyncHandler<ContentMovedToRecycleBinNotification> {
    private readonly Lazy<IBackgroundJob> _backgroundJob;
    private readonly IContentCache _contentCache;

    public PlatformsSettingsChangedOrRemovedNotification(Lazy<IBackgroundJob> backgroundJob, IContentCache contentCache) {
        _backgroundJob = backgroundJob;
        _contentCache = contentCache;
    }
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        if (notification.PublishedEntities.Any(x => x.IsPlatformsSubscriptionSettingContent(_contentCache))) {
            Enqueue(notification.PublishedEntities);
        }
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentUnpublishedNotification notification, CancellationToken cancellationToken) {
        if (notification.UnpublishedEntities.Any(x => x.IsPlatformsSubscriptionSettingContent(_contentCache))) {
            Enqueue(notification.UnpublishedEntities);
        }
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentMovedToRecycleBinNotification notification, CancellationToken cancellationToken) {
        if (notification.MoveInfoCollection.Select(x => x.Entity).Any(x => x.IsPlatformsSubscriptionSettingContent(_contentCache))) {
            Enqueue(notification.MoveInfoCollection.Select(x => x.Entity));
        }
        
        return Task.CompletedTask;
    }

    private void Enqueue(IEnumerable<IContent>  contents) {
        if (contents.Any(x => x.IsFundStructure(_contentCache))) {
            _backgroundJob.Value.EnqueueCommand<PublishPlatformsContentCommand>();
        }
        
        _backgroundJob.Value.EnqueueCommand<PublishPlatformsSubscriptionSettingsCommand>();
    }
}