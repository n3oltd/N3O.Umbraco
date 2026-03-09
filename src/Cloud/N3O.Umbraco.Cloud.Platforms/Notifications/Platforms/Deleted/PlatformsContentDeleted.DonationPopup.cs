using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class DonationPopupDeleted : PlatformsContentDeleted {
    public DonationPopupDeleted(ISubscriptionAccessor subscriptionAccessor,
                                ICloudUrl cloudUrl,
                                IBackgroundJob backgroundJob) 
        : base(subscriptionAccessor, cloudUrl, backgroundJob) { }

    protected override bool CanProcess(IContent content) {
        return content.IsDonationPopupElement();
    }
    
    protected override object GetBody(IContent content) {
        var donationPopupReq = new CustomElementWebhookBodyReqDonationPopupReq();
        donationPopupReq.Id = content.Key.ToString();
        donationPopupReq.Action = WebhookSyncAction.Deactivate;

        return donationPopupReq;
    }
    
    protected override string HookId => PlatformsConstants.WebhookIds.DonationPopups;
}