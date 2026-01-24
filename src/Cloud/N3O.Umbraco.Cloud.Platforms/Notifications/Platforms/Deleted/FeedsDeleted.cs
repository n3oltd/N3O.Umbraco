using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class FeedsDeleted : INotificationAsyncHandler<ContentMovingToRecycleBinNotification> {
    public Task HandleAsync(ContentMovingToRecycleBinNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.MoveInfoCollection.Select(x => x.Entity)) {
            if (content.IsFeeds()) {
                notification.CancelWithError("Feeds can not be deleted");
            }
        }
        return Task.CompletedTask;   
    }
}