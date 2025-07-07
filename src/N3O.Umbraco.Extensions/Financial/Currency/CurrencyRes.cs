using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Financial;

public class CurrencyRes : NamedLookupRes {
    public string Code { get; set; }
    public string Symbol { get; set; }
    public string Icon { get; set; }
    public int DecimalDigits { get; set; }
    public bool IsBaseCurrency { get; set; }
}
