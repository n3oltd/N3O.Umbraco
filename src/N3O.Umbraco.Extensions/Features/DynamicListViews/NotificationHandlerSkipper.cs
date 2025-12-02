using N3O.Umbraco.Notifications;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Features.DynamicListViews;

public class NotificationHandlerSkipper : INotificationHandlerSkipper {
    public bool ShouldSkip<TNotification>(INotificationAsyncHandler<TNotification> handler,
                                          TNotification notification)
        where TNotification : INotification {
        if (handler.GetType() == typeof(ApplicationStarted) ||
            handler.GetType() == typeof(ContentSending) ||
            handler.GetType() == typeof(ContentSaving) ||
            handler.GetType() == typeof(NodesRendering)) {
            return FeatureFlags.IsNotSet(FeatureFlags.DynamicListViews);
        }

        return false;
    }
}