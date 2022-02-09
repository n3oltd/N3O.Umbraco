using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class LastNameDataEntrySettings : TextFieldSettings {
        public LastNameDataEntrySettings(bool required, string label, string helpText, int order, Capitalisation capitalisation) : 
            base(required, label, helpText, order, capitalisation, true) { }
    }
}