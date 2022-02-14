using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class PhoneDataEntrySettings : Value, IFieldSettingsCollection {
        public PhoneDataEntrySettings(SelectFieldSettings country, PhoneFieldSettings number) {
            Country = country;
            Number = number;
        }

        public SelectFieldSettings Country { get; }
        public PhoneFieldSettings Number { get; }

        public IEnumerable<FieldSettings> GetFieldSettings() {
            yield return Country;
            yield return Number;
        }
    }
}