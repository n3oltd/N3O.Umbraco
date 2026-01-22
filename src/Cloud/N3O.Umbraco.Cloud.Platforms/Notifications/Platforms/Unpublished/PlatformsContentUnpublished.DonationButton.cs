using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class DonationButtonUnpublished : CloudContentUnpublished {
    public DonationButtonUnpublished(ISubscriptionAccessor subscriptionAccessor,
                                     ICloudUrl cloudUrl,
                                     IBackgroundJob backgroundJob)
        : base(subscriptionAccessor, cloudUrl, backgroundJob) {
    }
    
    protected override bool CanProcess(IContent content) {
        return content.IsDonationButtonElement();
    }

    protected override object GetBody(IContent content) {
        var donationButtonReq = new CustomElementWebhookBodyReqDonationButtonReq();
        donationButtonReq.Id = content.Key.ToString();
        donationButtonReq.Action = WebhookSyncAction.Deactivate;

        return donationButtonReq;
    }

    protected override string HookId => PlatformsConstants.WebhookIds.DonationButtons;
}