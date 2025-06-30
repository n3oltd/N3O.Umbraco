using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class PlatformsContentChangedOrRemovedNotification :
    INotificationAsyncHandler<ContentPublishedNotification>,
    INotificationAsyncHandler<ContentUnpublishedNotification>,
    INotificationAsyncHandler<ContentMovedToRecycleBinNotification> {
    private readonly Lazy<IBackgroundJob> _backgroundJob;
    private readonly IContentTypeService _contentTypeService;

    public PlatformsContentChangedOrRemovedNotification(Lazy<IBackgroundJob> backgroundJob,
                                                        IContentTypeService contentTypeService) {
        _backgroundJob = backgroundJob;
        _contentTypeService = contentTypeService;
    }
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        if (notification.PublishedEntities.Any(x => x.IsPlatformsCampaignOrDesignationOrElement(_contentTypeService))) {
            _backgroundJob.Value.EnqueueCommand<PublishPlatformsContentCommand>();
        }
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentUnpublishedNotification notification, CancellationToken cancellationToken) {
        if (notification.UnpublishedEntities.Any(x => x.IsPlatformsCampaignOrDesignationOrElement(_contentTypeService))) {
            _backgroundJob.Value.EnqueueCommand<PublishPlatformsContentCommand>();
        }
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentMovedToRecycleBinNotification notification, CancellationToken cancellationToken) {
        if (notification.MoveInfoCollection.Select(x => x.Entity).Any(x => x.IsPlatformsCampaignOrDesignationOrElement(_contentTypeService))) {
            _backgroundJob.Value.EnqueueCommand<PublishPlatformsContentCommand>();
        }
        
        return Task.CompletedTask;
    }
}