using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class PhoneDataEntrySettings : Value {
        public PhoneDataEntrySettings(bool required,
                                      string label,
                                      string helpText,
                                      Country defaultCountry,
                                      bool validate) {
            Required = required;
            Label = label;
            HelpText = helpText;
            DefaultCountry = defaultCountry;
            Validate = validate;
        }

        public bool Required { get; }
        public string Label { get; }
        public string HelpText { get; }
        public Country DefaultCountry { get; }
        public bool Validate { get; }
    }
}