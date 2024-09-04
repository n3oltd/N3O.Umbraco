using Konstrukt.Events;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Crowdfunding.UIBuilder; 

public class OnlineContributionSavingNotificationHandler :
    INotificationAsyncHandler<KonstruktEntitySavingNotification> {
    public Task HandleAsync(KonstruktEntitySavingNotification notification, CancellationToken cancellationToken) {
        if (notification.Entity.After is OnlineContribution after) {
            var before = (OnlineContribution) notification.Entity.Before;

            if (before.CheckoutReference != after.CheckoutReference) {
                notification.CancelWithError("Updating the reference of a contribution is not allowed");
            }
        }

        return Task.CompletedTask;
    }
}