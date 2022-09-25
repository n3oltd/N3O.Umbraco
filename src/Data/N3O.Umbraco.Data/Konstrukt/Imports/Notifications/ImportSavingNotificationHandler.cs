using Konstrukt.Events;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Data.Konstrukt.Notifications;

public class ImportSavingNotificationHandler : INotificationAsyncHandler<KonstruktEntitySavingNotification> {
    public Task HandleAsync(KonstruktEntitySavingNotification notification, CancellationToken cancellationToken) {
        if (notification.Entity.After is Import import && !import.CanProcess) {
            notification.CancelWithError("This record has been imported and can no longer be updated");
        }

        return Task.CompletedTask;
    }
}
