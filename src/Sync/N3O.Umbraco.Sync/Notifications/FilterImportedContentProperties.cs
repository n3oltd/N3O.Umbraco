using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using uSync.BackOffice;
using uSync.BackOffice.SyncHandlers.Handlers;

namespace N3O.Umbraco.Sync;

public class FilterImportedContentProperties : INotificationAsyncHandler<uSyncImportingItemNotification> {
    private readonly IEnumerable<IContentSyncFilter> _contentSyncFilters;

    public FilterImportedContentProperties(IEnumerable<IContentSyncFilter> contentSyncFilters) {
        _contentSyncFilters = contentSyncFilters.OrEmpty().ToList();
    }
    
    public Task HandleAsync(uSyncImportingItemNotification notification, CancellationToken cancellationToken) {
        if (notification.Handler is ContentHandler) {
            var info = notification.Item.Element("Info");
            var contentTypeAlias = info.Element("ContentType").Value;

            var filter = _contentSyncFilters.FirstOrDefault(x => x.IsFilter(contentTypeAlias));

            if (filter != null) {
                var properties = notification.Item.Element("Properties");

                foreach (var element in properties.Elements()) {
                    if (!filter.ShouldImport(element.Name.LocalName)) {
                        element.Remove();
                    }
                }
            }
        }

        return Task.CompletedTask;
    }
}