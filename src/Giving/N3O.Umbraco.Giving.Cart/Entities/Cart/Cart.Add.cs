using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Lookups;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public async Task AddAsync(IContentLocator contentLocator,
                               IForexConverter forexConverter,
                               IPriceCalculator priceCalculator,
                               ILookups lookups,
                               GivingType givingType,
                               IAllocation allocation,
                               int quantity = 1) {
        while (quantity > 0) {
            await ReplaceContentsAsync(contentLocator,
                                       forexConverter,
                                       priceCalculator,
                                       lookups,
                                       givingType,
                                       c => AddToContents(c, allocation));

            quantity--;
        }
    }

    private CartContents AddToContents(CartContents contents, IAllocation allocation) {
        var allocations = contents.Allocations.Concat(new Allocation(allocation)).ToList();

        return new CartContents(Currency, contents.Type, allocations);
    }
}
