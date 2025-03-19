using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Giving.Checkout.Entities;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class WebhookCheckoutExtensions {
    public static Checkout ToCheckout(this WebhookCheckout webhookCheckout, ILookups lookups) {
         var checkout = Checkout.Create(webhookCheckout.Revision.ToRevisionId(),
                                        webhookCheckout.CartRevisionId.ToRevisionId(),
                                        webhookCheckout.Reference.ToReference(),
                                        webhookCheckout.Currency.ToCurrency(lookups),
                                        webhookCheckout.Account.ToCheckoutAccount(lookups),
                                        webhookCheckout.Donation.ToDonationCheckout(lookups),
                                        null,
                                        null,
                                        webhookCheckout.RemoteIp);

         return checkout;
    }
}