using Konstrukt.Events;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Crowdfunding.UIBuilder; 

public class CrowdfundingContributionSavingNotificationHandler :
    INotificationAsyncHandler<KonstruktEntitySavingNotification> {
    public Task HandleAsync(KonstruktEntitySavingNotification notification, CancellationToken cancellationToken) {
        if (notification.Entity.After is CrowdfundingContribution after) {
            var before = (CrowdfundingContribution) notification.Entity.Before;

            if (before.CheckoutReference != after.CheckoutReference) {
                notification.CancelWithError("Updating the name/reference of a contribution is not allowed");
            }
        }

        return Task.CompletedTask;
    }
}