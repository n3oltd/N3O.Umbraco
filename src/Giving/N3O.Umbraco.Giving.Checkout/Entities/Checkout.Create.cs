using N3O.Umbraco.Context;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.References;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public static async Task<Checkout> CreateAsync(ICounters counters,
                                                   IRemoteIpAddressAccessor remoteIpAddressAccessor,
                                                   EntityId id,
                                                   Cart.Entities.Cart cart) {
        var checkout = Create<Checkout>(id);

        checkout.CartRevisionId = cart.RevisionId;
        checkout.Reference = await counters.NextAsync<CheckoutReferenceType>();
        checkout.Currency = cart.Currency;
        checkout.Donation = new DonationCheckout(cart.Donation.Allocations, cart.Currency);
        checkout.RegularGiving = new RegularGivingCheckout(cart.RegularGiving.Allocations, cart.Currency);
        checkout.Progress = new CheckoutProgress(checkout);
        checkout.RemoteIp = remoteIpAddressAccessor.GetRemoteIpAddress();

        return checkout;
    }
}
