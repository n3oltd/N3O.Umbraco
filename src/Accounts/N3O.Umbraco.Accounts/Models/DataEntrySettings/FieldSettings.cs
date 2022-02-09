namespace N3O.Umbraco.Accounts.Models {
    public class FieldSettings : Value {
        public FieldSettings(bool required, string label, string helpText, int order, bool visible) {
            Required = required;
            Label = label;
            HelpText = helpText;
            Order = order;
            Visible = visible;
        }

        public bool Required { get; }
        public string Label { get; }
        public string HelpText { get; }
        public int Order { get; }
        public bool Visible { get; }
    }
}