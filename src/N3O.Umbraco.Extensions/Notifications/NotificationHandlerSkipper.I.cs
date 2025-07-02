using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

public interface INotificationHandlerSkipper {
    bool ShouldSkip<TNotification>(INotificationAsyncHandler<TNotification> handler, TNotification notification)
        where TNotification : INotification;
}