using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Context;

public interface IBaseCurrencyAccessor {
    Currency GetBaseCurrency();
}
