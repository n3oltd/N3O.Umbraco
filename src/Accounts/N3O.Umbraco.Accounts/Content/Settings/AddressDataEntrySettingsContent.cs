using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.Accounts.Content {
    public class AddressDataEntrySettingsContent : UmbracoContent<AddressDataEntrySettingsContent> {
        public Country DefaultCountry => GetValue(x => x.DefaultCountry);
        public AddressFieldElement Line1 => GetValue(x => x.Line1);
        public AddressFieldElement Line2 => GetValue(x => x.Line2);
        public AddressFieldElement Line3 => GetValue(x => x.Line3);
        public AddressFieldElement Locality => GetValue(x => x.Locality);
        public AddressFieldElement AdministrativeArea => GetValue(x => x.AdministrativeArea);
        public AddressFieldElement PostalCode => GetValue(x => x.PostalCode);

        public AddressDataEntrySettings ToDataEntrySettings() {
            return new AddressDataEntrySettings(DefaultCountry,
                                                Line1.ToDataEntrySettings(HtmlField.Name<AccountReq>(x => x.Address.Line1)),
                                                Line2.ToDataEntrySettings(HtmlField.Name<AccountReq>(x => x.Address.Line2)),
                                                Line3.ToDataEntrySettings(HtmlField.Name<AccountReq>(x => x.Address.Line3)),
                                                Locality.ToDataEntrySettings(HtmlField.Name<AccountReq>(x => x.Address.Locality)),
                                                AdministrativeArea.ToDataEntrySettings(HtmlField.Name<AccountReq>(x => x.Address.AdministrativeArea)),
                                                PostalCode.ToDataEntrySettings(HtmlField.Name<AccountReq>(x => x.Address.PostalCode)));
        }
    }
}
