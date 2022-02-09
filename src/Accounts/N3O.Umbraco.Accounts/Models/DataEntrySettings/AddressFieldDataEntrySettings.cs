namespace N3O.Umbraco.Accounts.Models {
    public class AddressFieldDataEntrySettings : FieldSettings {
        public AddressFieldDataEntrySettings(bool visible, bool required, string label, string helpText, int order) : 
            base(required, label, helpText, order, visible) { }
    }
}