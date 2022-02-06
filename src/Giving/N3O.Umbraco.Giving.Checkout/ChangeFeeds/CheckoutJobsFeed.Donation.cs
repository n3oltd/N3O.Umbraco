using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Content;
using N3O.Umbraco.Giving.Checkout.Lookups;

namespace N3O.Umbraco.Giving.Checkout.ChangeFeeds {
    public partial class CheckoutJobsFeed {
        private void ProcessDonationJobs(EntityChange<Entities.Checkout> entityChange) {
            if (entityChange.Operation == EntityOperations.Update) {
                var isComplete = entityChange.SessionEntity.Donation.IsComplete;
                var wasComplete = entityChange.DatabaseEntity.Donation.IsComplete;

                if (isComplete && !wasComplete) {
                    var checkout = entityChange.SessionEntity;

                    _webhooks.Value.Queue(CheckoutWebhookEvents.DonationCompleteEvent, checkout);
                    SendEmail<DonationReceiptTemplateContent>(checkout);
                }
            }
        }
    }
}
