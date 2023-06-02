using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Telemetry;

public abstract class NotificationAsyncHandler<TNotification> : INotificationAsyncHandler<TNotification>
    where TNotification : INotification {
    private const string EventName = "ExecutedNotificationHandler";
    private const string Category = "NotificationHandler";
    
    private readonly IDurationWeightFinder _durationWeightFinder;
    private readonly ActivitySource _activitySource;

    protected NotificationAsyncHandler(ActivitySource activitySource = null,
                                       IDurationWeightFinder durationWeightFinder = null) {
        _durationWeightFinder = durationWeightFinder;
        _activitySource = activitySource;
    }

    public async Task HandleAsync(TNotification notification, CancellationToken cancellationToken) {
        using var activity = _activitySource.StartTimedActivity(_durationWeightFinder, EventName, Category);
        activity?.AddTag("Type", GetType().Name);

        await HandleNotificationAsync(notification, cancellationToken);
            
        activity?.Stop();
    }

    protected abstract Task HandleNotificationAsync(TNotification notification, CancellationToken cancellationToken);
}
