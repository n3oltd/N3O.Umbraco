using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public void Reset(Currency currency) {
        Currency = currency;
        Donation = CartContents.Create(currency, GivingTypes.Donation);
        RegularGiving = CartContents.Create(currency, GivingTypes.RegularGiving);
    }
}
