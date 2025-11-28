using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using static N3O.Umbraco.Features.DynamicListViews.DynamicListViewConstants;

namespace N3O.Umbraco.Features.DynamicListViews;

public class ContentSaving : INotificationAsyncHandler<ContentSavingNotification> {
    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (content.Properties.Contains(Properties.EnableDynamicListView)) {
                if (content.GetValue<bool>(Properties.EnableDynamicListView)) {
                    DynamicListViewsCache.Add(content.Id);
                } else {
                    DynamicListViewsCache.Remove(content.Id);
                }
            }
        }

        return Task.CompletedTask;
    }
}