using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Extensions; 

public static class CancelableObjectNotificationExtensions {
    public static void CancelWithError<T>(this CancelableObjectNotification<T> notification, string message)
        where T : class {
        notification.CancelOperation(new EventMessage("Error", message, EventMessageType.Error));
    }
}