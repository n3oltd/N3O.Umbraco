using N3O.Umbraco.Lookups;
using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.Accounts.Lookups {
    public class AccountsLookupTypes : ILookupTypesSet {
        [LookupInfo(typeof(ConsentCategory))]
        public const string ConsentCategories = "consentCategories";
        
        [LookupInfo(typeof(ConsentChannel))]
        public const string ConsentChannels = "consentChannels";
        
        [LookupInfo(typeof(ConsentResponse))]
        public const string ConsentResponses = "consentResponses";
        
        [LookupInfo(typeof(Country))]
        public const string Countries = "countries";

        [LookupInfo(typeof(TaxStatus))]
        public const string TaxStatuses = "taxStatuses";
    }
}