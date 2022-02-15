using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Accounts.Content {
    public class PhoneDataEntrySettingsContent : UmbracoContent<PhoneDataEntrySettingsContent> {
        public bool Required => GetValue(x => x.Required);
        public string Label => GetValue(x => x.Label);
        public string HelpText => GetValue(x => x.HelpText);
        public Country DefaultCountry => GetValue(x => x.DefaultCountry);
        public bool Validate => GetValue(x => x.Validate);

        public PhoneDataEntrySettings ToDataEntrySettings(ILookups lookups) {
            SelectOption ToSelectOption(Country country) => new(country.Id, $"{country.Name} ({country.DiallingCode})");
            
            var countryOptions = lookups.GetAll<Country>().Select(ToSelectOption).ToList();

            var countrySettings = new SelectFieldSettings(true,
                                                          Required,
                                                          Label,
                                                          HelpText,
                                                          HtmlField.Name<AccountReq>(x => x.Telephone.Country),
                                                          countryOptions,
                                                          1,
                                                          Validate,
                                                          DefaultCountry.IfNotNull(ToSelectOption));
            
            var numberSettings = new PhoneFieldSettings(true,
                                                        Required,
                                                        Label,
                                                        HelpText,
                                                        HtmlField.Name<AccountReq>(x => x.Telephone.Number),
                                                        1,
                                                        Validate);
            
            return new PhoneDataEntrySettings(countrySettings, numberSettings);
        }
    }
}
