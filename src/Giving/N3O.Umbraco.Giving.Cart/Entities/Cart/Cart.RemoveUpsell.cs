using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Models;
using System;
using System.Linq;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public void RemoveUpsell(CartContents contents, Guid upsellId) {
        var newAllocations = contents.Allocations.ToList();
        newAllocations.RemoveWhere(x => x.UpsellId == upsellId);

        Donation = new CartContents(Currency, contents.Type, newAllocations);
    }
}