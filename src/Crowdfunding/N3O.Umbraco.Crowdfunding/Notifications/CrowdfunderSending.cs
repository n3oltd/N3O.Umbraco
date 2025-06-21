using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Cloud.Engage.Lookups;
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
    private readonly Lazy<IWebHostEnvironment> _webHostEnvironment;

    public CrowdfunderSending(Lazy<ILookups> lookups,
                              Lazy<ICrowdfundingUrlBuilder> crowdfundingUrlBuilder,
                              Lazy<IWebHostEnvironment> webHostEnvironment) {
        _lookups = lookups;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
        _webHostEnvironment = webHostEnvironment;
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
                ProcessCampaignUrlLabel(variant);
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
    
    private void ProcessCampaignUrlLabel(ContentVariantDisplay variant) {
        if (_webHostEnvironment.Value.IsProduction()) {
            var calculatedTab = variant.Tabs.Single(x => x.Alias.EqualsInvariant("crowdfunder/calculated"));
            var campaignUrlProperty = GetProperty(calculatedTab, CrowdfundingConstants.Campaign.Properties.ProductionUrl);

            calculatedTab.Properties = calculatedTab.Properties.Except(campaignUrlProperty);
        }
    }

    private void ProcessStatus(ContentVariantDisplay variant) {
        var statusTab = variant.Tabs.Single(x => x.Alias.EqualsInvariant("crowdfunder/status"));
        var statusProperty = GetProperty(statusTab, CrowdfundingConstants.Crowdfunder.Properties.Status);
        var toggleStatusProperty = GetProperty(statusTab, CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus);

        var statusStr = (string) statusProperty.Value;
        var status = statusStr.HasValue() ? _lookups.Value.FindByName<CrowdfunderStatus>(statusStr).Single() : null;
        
        if (!status.HasValue()) {
            variant.Tabs = variant.Tabs.Except(statusTab);
        } else if (status.CanToggle) {
            toggleStatusProperty.Label = status.ToggleAction.Label;
        }
        
        // TODO Right now campaigns are activated by default and fundraisers from front end. There is no way to change
        // the Status on campaigns on production from staging. Hiding toggle so that users don't expect to change status
        // from staging. We do need to put this back at some stage so they can deactivate campaigns without logging into
        // production.
        statusTab.Properties = statusTab.Properties.Except(toggleStatusProperty);
    }

    private ContentPropertyDisplay GetProperty(Tab<ContentPropertyDisplay> tab, string alias) {
        return tab.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
    }
}