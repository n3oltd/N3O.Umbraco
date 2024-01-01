using Konstrukt.Events;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Crowdfunding.Konstrukt.Notifications; 

public class CrowdfundingContributionSavingNotificationHandler : INotificationAsyncHandler<KonstruktEntitySavingNotification> {
    public Task HandleAsync(KonstruktEntitySavingNotification notification, CancellationToken cancellationToken) {
        if (notification.Entity.After is CrowdfundingContribution after) {
            var before = (CrowdfundingContribution) notification.Entity.Before;

            if (before.CheckoutReference != after.CheckoutReference) {
                after.CheckoutReference = before.CheckoutReference;
            }
        }

        return Task.CompletedTask;
    }
}