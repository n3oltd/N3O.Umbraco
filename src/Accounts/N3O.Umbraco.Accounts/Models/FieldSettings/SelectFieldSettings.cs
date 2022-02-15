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
                                   SelectOption defaultOption = null) :
            base(visible, required, label, helpText, path, order, validate) {
            DefaultOption = defaultOption;
            Options = options;
        }

        public IEnumerable<SelectOption> Options { get; }
        public SelectOption DefaultOption { get; }

        public override string Type => "Select";
    }
}