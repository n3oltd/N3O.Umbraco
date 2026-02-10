using N3O.Umbraco.Scheduler;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public abstract class CloudContentPublished :
    PlatformsContentNotification, INotificationAsyncHandler<ContentPublishedNotification> {
    protected CloudContentPublished(ISubscriptionAccessor subscriptionAccessor,
                                    ICloudUrl cloudUrl,
                                    IBackgroundJob backgroundJob) 
        : base(subscriptionAccessor, cloudUrl, backgroundJob) { }

    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (CanProcess(content)) {
                var body = await GetBodyAsync(content);

                Enqueue(body);
            }
        }
    }

    protected abstract Task<object> GetBodyAsync(IContent content);
    protected abstract bool CanProcess(IContent content);
}