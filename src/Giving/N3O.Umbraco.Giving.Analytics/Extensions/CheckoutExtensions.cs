using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Analytics.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Analytics.Extensions;

public static class CheckoutExtensions {
    public static Purchase ToPurchase(this Checkout.Entities.Checkout checkout) {
        var items = new List<Item>();

        AddAllocations(items, checkout.Donation?.Allocations, GivingTypes.Donation);
        AddAllocations(items, checkout.RegularGiving?.Allocations, GivingTypes.RegularGiving);

        var purchase = new Purchase();
        purchase.Id = checkout.Reference.Text;
        purchase.Affiliation = "Website";
        purchase.Value = checkout.GetValue().Amount;
        purchase.Tax = 0;
        purchase.Shipping = 0;
        purchase.Currency = checkout.Currency.Code.ToUpper();
        purchase.Coupon = null;
        purchase.Items = items;

        return purchase;
    }

    private static void AddAllocations(List<Item> items,
                                       IEnumerable<Allocation> allocations,
                                       GivingType givingType) {
        var startIndex = items.Count;
        
        items.AddRange(allocations.OrEmpty().Select((x, i) => x.ToItem(givingType, startIndex + i)));
    }
}
