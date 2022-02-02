using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class TitleDataEntrySettings : Value {
        public TitleDataEntrySettings(bool required,
                                      string label,
                                      string helpText,
                                      int order,
                                      IEnumerable<string> options) {
            Required = required;
            Label = label;
            HelpText = helpText;
            Order = order;
            Options = options;
        }

        public bool Required { get; }
        public string Label { get; }
        public string HelpText { get; }
        public int Order { get; }
        public IEnumerable<string> Options { get; }
    }
}