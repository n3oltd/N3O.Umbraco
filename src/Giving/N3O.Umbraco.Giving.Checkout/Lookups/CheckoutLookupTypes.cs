using N3O.Umbraco.Lookups;
using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutLookupTypes : ILookupTypesSet {
        [LookupInfo(typeof(CheckoutStage))]
        public const string CheckoutStages = "checkoutStages";
            
        [LookupInfo(typeof(Country))]
        public const string Countries = "countries";
        
        [LookupInfo(typeof(TaxStatus))]
        public const string TaxStatuses = "taxStatuses";
    }
}