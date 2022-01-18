using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Financial {
    public class Currency : LookupContent<Currency> {
        public string Symbol => GetValue(x => x.Symbol);
        public bool IsBaseCurrency => GetValue(x => x.IsBaseCurrency);
        
        public double DecimalDigits => 2;
    }
}
