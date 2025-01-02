using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Extensions;
using uSync.BackOffice;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class USyncCampaignImporting : INotificationAsyncHandler<uSyncImportingItemNotification> {
    private static string[] ExcludedProperties = new[] {
        AliasHelper<CrowdfunderContent<CampaignContent>>.PropertyAlias(x => x.Status),
        AliasHelper<CrowdfunderContent<CampaignContent>>.PropertyAlias(x => x.ToggleStatus)
    };
    
    public Task HandleAsync(uSyncImportingItemNotification notification, CancellationToken cancellationToken) {
        var properties = notification.Item.Element("Properties");

        foreach(var element in properties.Elements()) {
            if (ExcludedProperties.InvariantContains(element.Name.LocalName))
                element.Remove();
        }
        
        return Task.CompletedTask;
    }
}