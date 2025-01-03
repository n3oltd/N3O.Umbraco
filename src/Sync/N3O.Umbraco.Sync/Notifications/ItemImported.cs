﻿using N3O.Umbraco.Content;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using uSync.BackOffice;

namespace N3O.Umbraco.Sync;

public class ItemImported : INotificationAsyncHandler<uSyncImportedItemNotification> {
    private readonly IContentCache _contentCache;

    public ItemImported(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public Task HandleAsync(uSyncImportedItemNotification notification, CancellationToken cancellationToken) {
        _contentCache.Flush();
        
        return Task.CompletedTask;
    }
}