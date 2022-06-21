using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Lookups;
using System.Linq;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public void Add(GivingType givingType, IAllocation allocation, int quantity = 1) {
        while (quantity > 0) {
            ReplaceContents(givingType, c => AddToContents(c, allocation));

            quantity--;
        }
    }

    private CartContents AddToContents(CartContents contents, IAllocation allocation) {
        var allocations = contents.Allocations.Concat(new Allocation(allocation)).ToList();

        return new CartContents(Currency, contents.Type, allocations);
    }
}
