using N3O.Umbraco.Scheduler;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public abstract class CloudContentUnpublished : CloudContentNotification, INotificationAsyncHandler<ContentUnpublishedNotification> {
    protected CloudContentUnpublished(ISubscriptionAccessor subscriptionAccessor,
                                      ICloudUrl cloudUrl,
                                      IBackgroundJob backgroundJob) 
        : base(subscriptionAccessor, cloudUrl, backgroundJob) { }

    public Task HandleAsync(ContentUnpublishedNotification notification, CancellationToken cancellationToken) {
        /*foreach (var content in notification.UnpublishedEntities) {
            if (CanProcess(content)) {
                var body = GetBody(content);

                Enqueue(body);
            }
        }*/
        
        return Task.CompletedTask;
    }

    protected abstract object GetBody(IContent content);
    protected abstract bool CanProcess(IContent content);
    protected abstract override string HookId { get; }
}