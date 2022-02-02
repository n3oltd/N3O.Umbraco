namespace N3O.Umbraco.Accounts.Models {
    public class AddressFieldDataEntrySettings : Value {
        public AddressFieldDataEntrySettings(bool visible, bool required, string label, string helpText, int order) {
            Visible = visible;
            Required = required;
            Label = label;
            HelpText = helpText;
            Order = order;
        }

        public bool Visible { get; }
        public bool Required { get; }
        public string Label { get; }
        public string HelpText { get; }
        public int Order { get; }
    }
}