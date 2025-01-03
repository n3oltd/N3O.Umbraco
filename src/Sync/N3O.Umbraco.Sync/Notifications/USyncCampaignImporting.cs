﻿using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using uSync.BackOffice;

namespace N3O.Umbraco.Sync;

public class USyncCampaignImporting : INotificationAsyncHandler<uSyncImportingItemNotification> {
    private readonly IEnumerable<IContentSyncFilter> _contentSyncFilters;

    public USyncCampaignImporting(IEnumerable<IContentSyncFilter> contentSyncFilters) {
        _contentSyncFilters = contentSyncFilters.OrEmpty().ToList();
    }
    
    public Task HandleAsync(uSyncImportingItemNotification notification, CancellationToken cancellationToken) {
        var contentTypeAlias = "crowdfundingCampaign";

        var filter = _contentSyncFilters.FirstOrDefault(x => x.IsFilter(contentTypeAlias));

        if (filter != null) {
            var properties = notification.Item.Element("Properties");
        
            foreach(var element in properties.Elements()) {
                if (!filter.ShouldImport(element.Name.LocalName)) {
                    element.Remove();
                }
            }
        }
        
        return Task.CompletedTask;
    }
}