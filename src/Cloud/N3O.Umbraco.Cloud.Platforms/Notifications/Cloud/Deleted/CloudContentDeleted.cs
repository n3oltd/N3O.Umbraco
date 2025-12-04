using N3O.Umbraco.Scheduler;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public abstract class CloudContentDeleted : CloudContentNotification, INotificationAsyncHandler<ContentMovedToRecycleBinNotification> {
    protected CloudContentDeleted(ISubscriptionAccessor subscriptionAccessor,
                                  IBackgroundJob backgroundJob) 
        : base(subscriptionAccessor, backgroundJob) { }

    public Task HandleAsync(ContentMovedToRecycleBinNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.MoveInfoCollection.Select(x => x.Entity)) {
            if (CanProcess(content)) {
                var body = GetBody(content);

                Enqueue(body);
            }
        }
        
        return Task.CompletedTask;
    }

    protected abstract Task<object> GetBody(IContent content);
    protected abstract bool CanProcess(IContent content);
    protected abstract override string HookId { get; }
}