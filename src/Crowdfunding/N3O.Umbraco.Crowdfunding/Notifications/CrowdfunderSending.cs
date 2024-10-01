using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CrowdfunderSending : INotificationAsyncHandler<SendingContentNotification> {
    private readonly ILookups _lookups;

    public CrowdfunderSending(ILookups lookups) {
        _lookups = lookups;
    }
    
    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var isCrowdfunder = notification.Content.ContentTypeAlias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias) || 
                            notification.Content.ContentTypeAlias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias);
        
        if (isCrowdfunder) {
            foreach (var variant in notification.Content.Variants) {
                ProcessStatus(variant);
            }
        }
        
        return Task.CompletedTask;
    }

    private void ProcessStatus(ContentVariantDisplay variant) {
        var statusTab = variant.Tabs.Single(x => x.Alias.EqualsInvariant("Status"));
        var statusProperty = GetProperty(statusTab, CrowdfundingConstants.Crowdfunder.Properties.Status);
        var toggleStatusProperty = GetProperty(statusTab, CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus);

        var statusStr = (string) statusProperty.Value;
        var status = statusStr.HasValue() ? _lookups.FindByName<CrowdfunderStatus>(statusStr).Single() : null;
        
        if (!status.HasValue()) {
            variant.Tabs = variant.Tabs.Except(statusTab);
        } else if (status.CanToggle) {
            toggleStatusProperty.Label = status.ToggleAction.Label;
        }
    }

    private ContentPropertyDisplay GetProperty(Tab<ContentPropertyDisplay> tab, string alias) {
        return tab.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
    }
}