using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class FirstNameDataEntrySettings : TextFieldSettings {
        public FirstNameDataEntrySettings(bool required, string label, string helpText, int order, Capitalisation capitalisation) : 
            base(required, label, helpText, order, capitalisation, true) { }
    }
}