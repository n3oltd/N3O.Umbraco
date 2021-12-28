using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Financial;

public class MoneyReq {
    [Name("Amount")]
    public decimal? Amount { get; set; }
    
    [Name("Currency")]
    public Currency Currency { get; set; }
    
    public static implicit operator Money(MoneyReq req) {
        return new Money(req.Amount.GetValueOrThrow(), req.Currency);
    }
}