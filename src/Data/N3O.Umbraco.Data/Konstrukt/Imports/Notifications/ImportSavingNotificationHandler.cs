using Konstrukt.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Data.Konstrukt.Notifications {
    public class ImportSavingNotificationHandler : INotificationAsyncHandler<KonstruktEntitySavingNotification> {
        public Task HandleAsync(KonstruktEntitySavingNotification notification, CancellationToken cancellationToken) {
            if (notification.Entity.After is Import import && !import.CanProcess) {
                //Below does not show message in current version of Konstrukt
                /*notification.Cancel = true;
                notification.Messages.Add(new EventMessage("Error",
                                                           "This record has been imported and can no longer be updated",
                                                           EventMessageType.Error));*/
                throw new Exception("This record has been imported and can no longer be updated");
            }

            return Task.CompletedTask;
        }
    }
}