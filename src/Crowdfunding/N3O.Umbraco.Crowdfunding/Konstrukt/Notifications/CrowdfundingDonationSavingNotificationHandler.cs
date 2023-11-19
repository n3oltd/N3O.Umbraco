using Konstrukt.Events;
using N3O.Umbraco.CrowdFunding.Konstrukt;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.CrowdFunding.Konstrukt.Notifications;

public class CrowdfundingDonationSavingNotificationHandler : INotificationAsyncHandler<KonstruktEntitySavingNotification> {
    public Task HandleAsync(KonstruktEntitySavingNotification notification, CancellationToken cancellationToken) {
        if (notification.Entity.After is CrowdfundingDonation crowdfundingDonation) {
            var before = (CrowdfundingDonation) notification.Entity.Before;
            var after = (CrowdfundingDonation) notification.Entity.After;

            if (before.Reference != after.Reference) {
                after.Reference = before.Reference;
            }
        }

        return Task.CompletedTask;
    }
}
