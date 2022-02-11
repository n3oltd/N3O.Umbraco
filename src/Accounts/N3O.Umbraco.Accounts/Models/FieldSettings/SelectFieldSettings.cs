using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class SelectFieldSettings : FieldSettings {
        public SelectFieldSettings(bool visible,
                                   bool required,
                                   string label,
                                   string helpText,
                                   string path,
                                   IEnumerable<string> options,
                                   int order = -1,
                                   bool validate = false) :
            base(visible, required, label, helpText, path, order, validate) {
            Options = options;
        }

        public IEnumerable<string> Options { get; }
        
        public override string Type => "Select";
    }
}