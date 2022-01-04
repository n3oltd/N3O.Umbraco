using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Giving.Cart {
    public interface ICartValidator {
        bool IsValid(Currency currentCurrency, Entities.Cart cart);
    }
}