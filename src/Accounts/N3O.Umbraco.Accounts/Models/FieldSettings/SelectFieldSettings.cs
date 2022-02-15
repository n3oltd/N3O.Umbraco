using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class SelectFieldSettings : FieldSettings {
        public SelectFieldSettings(bool visible,
                                   bool required,
                                   string label,
                                   string helpText,
                                   string path,
                                   IEnumerable<SelectOption> options,
                                   int order = -1,
                                   bool validate = false,
                                   string defaultValue = null) :
            base(visible, required, label, helpText, path, order, validate) {
            DefaultValue = defaultValue;
            Options = options;
        }

        public IEnumerable<SelectOption> Options { get; }
        public string DefaultValue { get; }

        public override string Type => "Select";
    }
}