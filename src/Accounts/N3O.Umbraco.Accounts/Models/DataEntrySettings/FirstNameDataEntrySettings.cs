using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class FirstNameDataEntrySettings : Value {
        public FirstNameDataEntrySettings(bool required,
                                          string label,
                                          string helpText,
                                          int order,
                                          Capitalisation capitalisation) {
            Required = required;
            Label = label;
            HelpText = helpText;
            Order = order;
            Capitalisation = capitalisation;
        }

        public bool Required { get; }
        public string Label { get; }
        public string HelpText { get; }
        public int Order { get; }
        public Capitalisation Capitalisation { get; }
    }
}