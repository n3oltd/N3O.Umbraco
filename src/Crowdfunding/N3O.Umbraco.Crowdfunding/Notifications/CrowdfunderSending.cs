using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CrowdfunderSending : INotificationAsyncHandler<SendingContentNotification> {
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<ICrowdfundingUrlBuilder> _crowdfundingUrlBuilder;

    public CrowdfunderSending(Lazy<ILookups> lookups, Lazy<ICrowdfundingUrlBuilder> crowdfundingUrlBuilder) {
        _lookups = lookups;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
    }
    
    public Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken) {
        var isCampaign = notification.Content.ContentTypeAlias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias);
        var isFundraiser = notification.Content.ContentTypeAlias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias);
        
        if (isFundraiser || isCampaign) {
            var url = isCampaign ? GetCampaignUrl(notification.Content) : GetFundraiserUrl(notification.Content);

            if (url.HasValue()) {
                notification.Content.Urls = [new UrlInfo(url, true, null)];
            }

            foreach (var variant in notification.Content.Variants) {
                ProcessStatus(variant);
            }
        }
        
        return Task.CompletedTask;
    }

    private string GetFundraiserUrl(ContentItemDisplay notificationContent) {
        return notificationContent.Key.IfNotNull(x => ViewEditFundraiserPage.Url(_crowdfundingUrlBuilder.Value, x));
    }

    private string GetCampaignUrl(ContentItemDisplay notificationContent) {
        return notificationContent.Key.IfNotNull(x => ViewCampaignPage.Url(_crowdfundingUrlBuilder.Value, x));
    }

    private void ProcessStatus(ContentVariantDisplay variant) {
        var statusTab = variant.Tabs.Single(x => x.Alias.EqualsInvariant("Status"));
        var statusProperty = GetProperty(statusTab, CrowdfundingConstants.Crowdfunder.Properties.Status);
        var toggleStatusProperty = GetProperty(statusTab, CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus);

        var statusStr = (string) statusProperty.Value;
        var status = statusStr.HasValue() ? _lookups.Value.FindByName<CrowdfunderStatus>(statusStr).Single() : null;
        
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