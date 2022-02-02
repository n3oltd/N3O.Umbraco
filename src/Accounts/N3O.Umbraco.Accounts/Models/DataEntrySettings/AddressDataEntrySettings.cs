using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class AddressDataEntrySettings {
        public AddressDataEntrySettings(Country defaultCountry,
                                        AddressFieldDataEntrySettings line1,
                                        AddressFieldDataEntrySettings line2,
                                        AddressFieldDataEntrySettings line3,
                                        AddressFieldDataEntrySettings locality,
                                        AddressFieldDataEntrySettings administrativeArea,
                                        AddressFieldDataEntrySettings postalCode) {
            DefaultCountry = defaultCountry;
            Line1 = line1;
            Line2 = line2;
            Line3 = line3;
            Locality = locality;
            AdministrativeArea = administrativeArea;
            PostalCode = postalCode;
        }

        public Country DefaultCountry { get; }
        public AddressFieldDataEntrySettings Line1 { get; }
        public AddressFieldDataEntrySettings Line2 { get; }
        public AddressFieldDataEntrySettings Line3 { get; }
        public AddressFieldDataEntrySettings Locality { get; }
        public AddressFieldDataEntrySettings AdministrativeArea { get; }
        public AddressFieldDataEntrySettings PostalCode { get; }
        
    }
}