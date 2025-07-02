using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

public class NotificationAsyncHandlerWrapper<TNotification> : INotificationAsyncHandler<TNotification>
    where TNotification : INotification {
    private readonly IReadOnlyList<INotificationHandlerSkipper> _notificationHandlerSkippers;
    private readonly INotificationAsyncHandler<TNotification> _wrappedNotificationHandler;

    public NotificationAsyncHandlerWrapper(IEnumerable<INotificationHandlerSkipper> notificationHandlerSkippers,
                                           INotificationAsyncHandler<TNotification> wrappedNotificationHandler) {
        _notificationHandlerSkippers = notificationHandlerSkippers.OrEmpty().ApplyAttributeOrdering();
        _wrappedNotificationHandler = wrappedNotificationHandler;
    }
    
    public async Task HandleAsync(TNotification notification, CancellationToken cancellationToken) {
        foreach (var notificationHandlerSkipper in _notificationHandlerSkippers) {
            if (notificationHandlerSkipper.ShouldSkip(this, notification)) {
                return;
            }
        }
        
        await _wrappedNotificationHandler.HandleAsync(notification, cancellationToken);
    }
}