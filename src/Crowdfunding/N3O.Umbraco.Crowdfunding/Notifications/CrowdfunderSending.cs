using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CrowdfunderSending : INotificationAsyncHandler<SendingContentNotification> {
    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var isCrowdfunder = notification.Content.ContentTypeAlias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias) || 
                            notification.Content.ContentTypeAlias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias);
        
        if (isCrowdfunder) {
            foreach (var variant in notification.Content.Variants) {
                ProcessCrowdfunderStatus(variant);
            }
        }
        
        return Task.CompletedTask;
    }

    private void ProcessCrowdfunderStatus(ContentVariantDisplay variant) {
        var statusTab = variant.Tabs.Single(x => x.Alias.EqualsInvariant("Status"));
        
        var status = GetProperty(statusTab, CrowdfundingConstants.Crowdfunder.Properties.Status);
        var toggleStatus = GetProperty(statusTab, CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus);

        var statusValue = (string) status.Value;
        
        if (!status.HasValue()) {
            statusTab.Properties = statusTab.Properties?.Except([status, toggleStatus]);
        }
                
        if (statusValue.IsAnyOf(CrowdfunderStatuses.Draft.Name, CrowdfunderStatuses.Inactive.Name)) {
            toggleStatus.Label = "Activate Campaign";
        }
                
        if (statusValue == CrowdfunderStatuses.Active.Name) {
            toggleStatus.Label = "Deactivate Campaign";
        }
    }

    private ContentPropertyDisplay GetProperty(Tab<ContentPropertyDisplay> tab, string alias) {
        return tab.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
    }
}