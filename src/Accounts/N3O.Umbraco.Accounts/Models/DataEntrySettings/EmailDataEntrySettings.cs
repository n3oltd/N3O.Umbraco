namespace N3O.Umbraco.Accounts.Models {
    public class EmailDataEntrySettings : Value {
        public EmailDataEntrySettings(bool required, string label, string helpText, bool validate) {
            Required = required;
            Label = label;
            HelpText = helpText;
            Validate = validate;
        }

        public bool Required { get; }
        public string Label { get; }
        public string HelpText { get; }
        public bool Validate { get; }
    }
}