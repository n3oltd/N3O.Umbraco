using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

// https://docs.jumoo.co.uk/usync/uSync/extending/detecting/#detecting-via-notification-state-usync-v131
public class SyncNotificationHandlerSkipper : INotificationHandlerSkipper {
    private const string uSyncEventStateKey = "uSync.ProcessState";
    private const string uSyncEventPausedKey = "uSync.PausedKey";
    
    public bool ShouldSkip<TNotification>(INotificationAsyncHandler<TNotification> handler,
                                          TNotification notification)
        where TNotification : INotification {
        if (notification is IStatefulNotification statefulNotification &&
            handler.GetType().HasAttribute<SkipDuringSyncAttribute>()) {
            var uSyncOrigin = default(object);
            var uSyncPaused = default(object);
            
            if (statefulNotification.State.TryGetValue(uSyncEventStateKey, out uSyncOrigin) ||
                statefulNotification.State.TryGetValue(uSyncEventPausedKey, out uSyncPaused)) {
                if (uSyncOrigin is true || uSyncPaused is true) {
                    return true;
                }
            }
        }

        return false;
    }
}