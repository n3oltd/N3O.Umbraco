using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class AddressDataEntrySettings : Value, IFieldSettingsCollection {
        public AddressDataEntrySettings(Country defaultCountry,
                                        TextFieldSettings line1,
                                        TextFieldSettings line2,
                                        TextFieldSettings line3,
                                        TextFieldSettings locality,
                                        TextFieldSettings administrativeArea,
                                        TextFieldSettings postalCode) {
            DefaultCountry = defaultCountry;
            Line1 = line1;
            Line2 = line2;
            Line3 = line3;
            Locality = locality;
            AdministrativeArea = administrativeArea;
            PostalCode = postalCode;
        }

        public Country DefaultCountry { get; }
        public TextFieldSettings Line1 { get; }
        public TextFieldSettings Line2 { get; }
        public TextFieldSettings Line3 { get; }
        public TextFieldSettings Locality { get; }
        public TextFieldSettings AdministrativeArea { get; }
        public TextFieldSettings PostalCode { get; }
        
        public IEnumerable<FieldSettings> GetFieldSettings() {
            yield return Line1;
            yield return Line2;
            yield return Line3;
            yield return Locality;
            yield return AdministrativeArea;
            yield return PostalCode;
        }
    }
}