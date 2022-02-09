using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class TitleDataEntrySettings : OptionFieldSettting {
        public TitleDataEntrySettings(bool required, string label, string helpText, int order, IEnumerable<string> options) :
            base(required, label, helpText, order, true, options) { }
    }
}