using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutLookupTypes : ILookupTypesSet {
        [LookupInfo(typeof(Country))]
        public const string Countries = "countries";
    }
}