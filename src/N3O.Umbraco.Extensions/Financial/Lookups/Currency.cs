using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Financial {
    public class Currency : LookupContent<Currency> {
        public string Symbol => GetValue(x => x.Symbol);
        public double DecimalDigits => GetValue(x => x.DecimalDigits);
        public bool IsBaseCurrency => GetValue(x => x.IsBaseCurrency);
    }
}
