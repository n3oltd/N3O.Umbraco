using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Models;

namespace N3O.Umbraco.Giving.Cart;

public interface ICartValidator {
    bool IsValid(Currency currentCurrency, DonationCart cart);
}