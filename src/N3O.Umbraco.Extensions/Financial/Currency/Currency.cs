using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Financial;

public class Currency : LookupContent<Currency> {
    public string Code => Name.ToUpperInvariant();
    public bool IsBaseCurrency => GetValue(x => x.IsBaseCurrency);
    public string Symbol => GetValue(x => x.Symbol);
    public double DecimalDigits => 2;
}
