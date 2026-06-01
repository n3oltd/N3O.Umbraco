using Umbraco.UIBuilder.Events;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Data.UIBuilder.Notifications;

public class ImportSavedNotificationHandler : INotificationAsyncHandler<EntitySavedNotification> {
    private readonly IImportProcessingQueue _importProcessingQueue;

    public ImportSavedNotificationHandler(IImportProcessingQueue importProcessingQueue) {
        _importProcessingQueue = importProcessingQueue;
    }

    public Task HandleAsync(EntitySavedNotification notification, CancellationToken cancellationToken) {
        if (notification.Entity.After is Import import && import.CanProcess) {
            _importProcessingQueue.Add(import);
        }

        return Task.CompletedTask;
    }
}
