using Humanizer;
using N3O.Umbraco.Telemetry.Extensions;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Telemetry.Notifications;

public abstract class NotificationAsyncHandlerWithTelemetry<TNotification> : INotificationAsyncHandler<TNotification>
    where TNotification : INotification {
    private Type _type;
    
    public async Task HandleAsync(TNotification notification, CancellationToken cancellationToken) {
        _type = GetType();
        
        using (var activity = ActivitySource.StartTimedActivity(GetActivityName(), "notifications", Stopwatch)) {
            activity.AddBaggage("handlerType", GetType().FullName);
            
            activity.AddTag("notificationType", typeof(TNotification).FullName);

            await HandleNotificationAsync(notification, cancellationToken);
        }
    }

    private string GetActivityName() {
        var typeName = _type.Name.Camelize();

        if (typeName.EndsWith("Handler")) {
            typeName = typeName.Substring(0, typeName.Length - "Handler".Length);
        }
        
        return $"notifications/{typeName}";
    }

    protected abstract Task HandleNotificationAsync(TNotification notification, CancellationToken cancellationToken);
    
    protected virtual ActivitySource ActivitySource => ActivitySources.Get(_type);
    protected virtual ITelemetryStopwatch Stopwatch => null;
}
