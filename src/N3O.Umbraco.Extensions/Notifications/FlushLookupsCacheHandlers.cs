using N3O.Umbraco.Lookups;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

public class FlushLookupsCacheHandlers :
    INotificationAsyncHandler<ContentDeletedNotification>,
    INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly ILookups _lookups;

    public FlushLookupsCacheHandlers(ILookups lookups) {
        _lookups = lookups;
    }

    public Task HandleAsync(ContentDeletedNotification notification, CancellationToken cancellationToken) {
        Flush();
        
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        Flush();
        
        return Task.CompletedTask;
    }

    private void Flush() {
        _lookups.Flush();
    }
}
