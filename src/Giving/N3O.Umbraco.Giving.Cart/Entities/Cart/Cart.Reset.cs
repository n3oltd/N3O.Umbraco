using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public void Reset(Currency currency = null) {
        if (currency != null) {
            Currency = currency;
        }
        
        Donation = CartContents.Create(Currency, GivingTypes.Donation);
        RegularGiving = CartContents.Create(Currency, GivingTypes.RegularGiving);
    }
}
