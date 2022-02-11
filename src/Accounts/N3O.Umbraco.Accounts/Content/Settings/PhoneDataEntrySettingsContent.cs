using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.Accounts.Content {
    public class PhoneDataEntrySettingsContent : UmbracoContent<PhoneDataEntrySettingsContent> {
        public bool Required => GetValue(x => x.Required);
        public string Label => GetValue(x => x.Label);
        public string HelpText => GetValue(x => x.HelpText);
        public Country DefaultCountry => GetValue(x => x.DefaultCountry);
        public bool Validate => GetValue(x => x.Validate);

        public PhoneDataEntrySettings ToDataEntrySettings() {
            return new PhoneDataEntrySettings(true, 
                                              Required,
                                              Label,
                                              HelpText,
                                              HtmlField.Name<AccountReq>(x => x.Telephone.Number),
                                              1,
                                              Validate,
                                              DefaultCountry);
        }
    }
}
