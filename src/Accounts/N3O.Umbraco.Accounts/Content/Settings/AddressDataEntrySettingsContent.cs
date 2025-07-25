using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using System.Linq;

namespace N3O.Umbraco.Accounts.Content;

public class AddressDataEntrySettingsContent : UmbracoContent<AddressDataEntrySettingsContent> {
    public AddressFieldElement Country => GetValue(x => x.Country);
    public AddressFieldElement Line1 => GetValue(x => x.Line1);
    public AddressFieldElement Line2 => GetValue(x => x.Line2);
    public AddressFieldElement Line3 => GetValue(x => x.Line3);
    public AddressFieldElement Locality => GetValue(x => x.Locality);
    public AddressFieldElement AdministrativeArea => GetValue(x => x.AdministrativeArea);
    public AddressFieldElement PostalCode => GetValue(x => x.PostalCode);
    public string LookupApiKey => GetValue(x => x.LookupApiKey);
    public AddressLayout Layout => GetValue(x => x.Layout);

    public Country GetDefaultCountry(ILookups lookups) {
        return GetLookup<Country>(lookups, AccountsConstants.DataEntrySettings.Properties.DefaultCountry);
    }

    public AddressDataEntrySettings ToDataEntrySettings(ILookups lookups) {
        SelectOption ToSelectOption(Country country) {
            return new(country.Id, country.Name);
        }

        var allCountries = lookups.GetAll<Country>();
        var defaultCountry = GetDefaultCountry(lookups);
        var countryOptions = allCountries.Select(ToSelectOption).ToList();
        var countryField = Country.ToSelectFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Country),
                                                         countryOptions,
                                                         defaultCountry.IfNotNull(ToSelectOption));
        
        return new AddressDataEntrySettings(countryField,
                                            Line1.ToTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Line1)),
                                            Line2.ToTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Line2)),
                                            Line3.ToTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Line3)),
                                            Locality.ToTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.Locality)),
                                            AdministrativeArea.ToTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.AdministrativeArea)),
                                            PostalCode.ToTextFieldSettings(HtmlField.Name<AccountReq>(x => x.Address.PostalCode)),
                                            LookupApiKey,
                                            defaultCountry,
                                            Layout);
    }
}
