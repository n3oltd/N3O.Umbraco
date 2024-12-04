using N3O.Umbraco.Content;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Cart.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public async Task RemoveAllAsync(IContentLocator contentLocator,
                                     IForexConverter forexConverter,
                                     IPriceCalculator priceCalculator,
                                     GivingType givingType) {
        await ReplaceContentsAsync(contentLocator,
                                   forexConverter,
                                   priceCalculator,
                                   givingType,
                                   _ => CartContents.Create(Currency, givingType));
    }
}
