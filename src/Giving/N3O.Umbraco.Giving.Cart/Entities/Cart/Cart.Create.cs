using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Cart.Models;

namespace N3O.Umbraco.Giving.Cart.Entities {
    public partial class Cart {
        public static Cart Create(Currency currency) {
            var cart = new Cart();
            cart.Currency = currency;
            cart.Single = CartContents.Create(currency, DonationTypes.Single);
            cart.Regular = CartContents.Create(currency, DonationTypes.Regular);

            return cart;
        }
    }
}