using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using uSync.BackOffice;

namespace N3O.Umbraco.Sync;

public class USyncItemImported : INotificationAsyncHandler<uSyncImportedItemNotification> {
    private readonly IContentCache _contentCache;

    public USyncItemImported(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public Task HandleAsync(uSyncImportedItemNotification notification, CancellationToken cancellationToken) {
        var contentTypeAlias = notification.Item.Attribute("Alias")?.Value;

        if (contentTypeAlias.HasValue()) {
            _contentCache.Flush(contentTypeAlias);
        }
        
        return Task.CompletedTask;
    }
}