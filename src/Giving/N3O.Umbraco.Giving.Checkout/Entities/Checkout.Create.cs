using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.References;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public static async Task<Checkout> CreateAsync(ICounters counters, EntityId id, Cart.Entities.Cart cart) {
            var checkout = Create<Checkout>(id);

            checkout.CartId = cart.Id;
            checkout.Reference = await counters.NextAsync<CheckoutReferenceType>();
            checkout.Currency = cart.Currency;

            IEnumerable<CheckoutStage> requiredStages = new[] { CheckoutStages.Account, CheckoutStages.RegularGiving };
            
            if (!cart.Donation.IsEmpty()) {
                requiredStages = requiredStages.Concat(CheckoutStages.Donation);
                checkout.Donation = new DonationCheckout(cart.Donation.Allocations);
            }
            
            if (!cart.RegularGiving.IsEmpty()) {
                requiredStages = requiredStages.Concat(CheckoutStages.RegularGiving);
                checkout.Donation = new DonationCheckout(cart.RegularGiving.Allocations);
            }
            
            checkout.Progress = new CheckoutProgress(requiredStages);

            return checkout;
        }
    }
}