using N3O.Umbraco.Content;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using uSync.BackOffice;

namespace N3O.Umbraco.Sync;

public class UsyncItemImported : INotificationAsyncHandler<uSyncImportedItemNotification> {
    private readonly IContentCache _contentCache;

    public UsyncItemImported(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public Task HandleAsync(uSyncImportedItemNotification notification, CancellationToken cancellationToken) {
        var alias = notification.Item.Attribute("Alias")?.Value;
        
        _contentCache.Flush(alias);
        
        return Task.CompletedTask;
    }
}