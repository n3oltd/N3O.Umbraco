using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class DonationFormDeleted : PlatformsContentDeleted {
    public DonationFormDeleted(ISubscriptionAccessor subscriptionAccessor,
                               ICloudUrl cloudUrl,
                               IBackgroundJob backgroundJob) 
        : base(subscriptionAccessor, cloudUrl, backgroundJob) { }

    protected override bool CanProcess(IContent content) {
        return content.IsDonationFormElement();
    }
    
    protected override object GetBody(IContent content) {
        var donationFormReq = new CustomElementWebhookBodyReqDonationFormReq();
        donationFormReq.Id = content.Key.ToString();
        donationFormReq.Action = WebhookSyncAction.Deactivate;

        return donationFormReq;
    }

    protected override string HookId => PlatformsConstants.WebhookIds.DonationForms;
}