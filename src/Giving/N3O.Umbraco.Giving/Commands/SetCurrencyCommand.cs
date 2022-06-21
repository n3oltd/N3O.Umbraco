using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Commands;

public class SetCurrencyCommand : Request<None, CurrencyRes> {
    public SetCurrencyCommand(CurrencyCode currencyCode) {
        CurrencyCode = currencyCode;
    }
    
    public CurrencyCode CurrencyCode { get; }
}
