using N3O.Umbraco.Counters;
using N3O.Umbraco.Giving.Checkout.Lookups;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public static async Task<Checkout> CreateAsync(ICounters counters, Guid id, Cart.Entities.Cart cart) {
            var checkout = Create<Checkout>(id);

            checkout.Reference = await counters.NextAsync<CheckoutReferenceType>();
            checkout.CartId = cart.Id;
            // TODO

            return checkout;
        }
    }
}