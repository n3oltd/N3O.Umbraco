using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class OptionFieldSettting : FieldSettings {
        public OptionFieldSettting(bool required, string label, string helpText, int order, bool visible, IEnumerable<string> options) :
            base(required, label, helpText, order, visible) {
            Options = options;
        }

        public IEnumerable<string> Options { get; }
    }
}