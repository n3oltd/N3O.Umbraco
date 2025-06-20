using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class PlatformEntityPublishedNotification : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IBackgroundJob _backgroundJob;
    private readonly IContentLocator _contentLocator;

    public PlatformEntityPublishedNotification(IBackgroundJob backgroundJob, IContentLocator contentLocator) {
        _backgroundJob = backgroundJob;
        _contentLocator = contentLocator;
    }
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        if (notification.PublishedEntities.Any(x => x.IsPlatformEntity(_contentLocator))) {
            _backgroundJob.Enqueue<PublishPlatformsContentCommand>($"{nameof(PublishPlatformsContentCommand).Replace("Command", "")}");
        }
        
        return Task.CompletedTask;
    }
}