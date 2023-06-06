using N3O.Umbraco.Telemetry.Extensions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Telemetry.Notifications;

public abstract class NotificationAsyncHandlerWithTelemetry<TNotification> : INotificationAsyncHandler<TNotification>
    where TNotification : INotification {
    public async Task HandleAsync(TNotification notification, CancellationToken cancellationToken) {
        using (var activity = ActivitySource?.StartTimedActivity("RunNotificationHandler",
                                                                 "Notifications",
                                                                 DurationBucketer)) {
            activity?.AddTag("HandlerType", GetType().FullName);

            await HandleNotificationAsync(notification, cancellationToken);
        }
    }

    protected abstract Task HandleNotificationAsync(TNotification notification, CancellationToken cancellationToken);
    
    protected virtual ActivitySource ActivitySource => new(GetType().Assembly.FullName);
    protected virtual IActivityDurationBucketer DurationBucketer => null;
}
