using N3O.Umbraco.Accounts.Extensions;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using System.Linq;

namespace N3O.Umbraco.Accounts.Content {
    public class AddressDataEntrySettingsContent : UmbracoContent<AddressDataEntrySettingsContent> {
        public AddressFieldElement Country => GetValue(x => x.Country);
        public Country DefaultCountry => GetValue(x => x.DefaultCountry);
        public AddressFieldElement Line1 => GetValue(x => x.Line1);
        public AddressFieldElement Line2 => GetValue(x => x.Line2);
        public AddressFieldElement Line3 => GetValue(x => x.Line3);
        public AddressFieldElement Locality => GetValue(x => x.Locality);
        public AddressFieldElement AdministrativeArea => GetValue(x => x.AdministrativeArea);
        public AddressFieldElement PostalCode => GetValue(x => x.PostalCode);

        public AddressDataEntrySettings ToDataEntrySettings(ILookups lookups) {
            var countryOptions = lookups.GetAll<Country>().Select(x => new SelectOption(x.Id, x.Name)).ToList();
            
            var countryField = Country.ToDataEntrySelectFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Country), countryOptions, DefaultCountry.Id);
            
            return new AddressDataEntrySettings(countryField,
                                                DefaultCountry,
                                                Line1.ToDataEntryTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Line1)),
                                                Line2.ToDataEntryTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Line2)),
                                                Line3.ToDataEntryTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Line3)),
                                                Locality.ToDataEntryTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Locality)),
                                                AdministrativeArea.ToDataEntryTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.AdministrativeArea)),
                                                PostalCode.ToDataEntryTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.PostalCode)));
        }
    }
}
