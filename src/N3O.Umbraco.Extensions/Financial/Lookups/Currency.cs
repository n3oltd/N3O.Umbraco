using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Financial {
    public class Currency : LookupContent {
        public string Symbol => GetValue<Currency, string>(x => x.Symbol);
        public double DecimalDigits => GetValue<Currency, double>(x => x.DecimalDigits);
        public bool IsBaseCurrency => GetValue<Currency, bool>(x => x.IsBaseCurrency);
    }
}
