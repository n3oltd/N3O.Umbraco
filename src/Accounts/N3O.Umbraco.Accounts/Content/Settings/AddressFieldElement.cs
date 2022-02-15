using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Content {
    public class AddressFieldElement : UmbracoElement<AddressFieldElement> {
        public bool Visible => GetValue(x => x.Visible);
        public bool Required => GetValue(x => x.Required);
        public string Label => GetValue(x => x.Label);
        public string HelpText => GetValue(x => x.HelpText);
        public int Order => GetValue(x => x.Order);

        public TextFieldSettings ToTextFieldSettings(string path) {
            return new TextFieldSettings(Visible, Required, Label, HelpText, path, Order);
        }

        public SelectFieldSettings ToSelectFieldSettings(string path,
                                                         IEnumerable<SelectOption> options,
                                                         SelectOption defaultOption = null) {
            return new SelectFieldSettings(Visible,
                                           Required,
                                           Label,
                                           HelpText,
                                           path,
                                           options,
                                           Order,
                                           false,
                                           defaultOption);
        }
    }
}