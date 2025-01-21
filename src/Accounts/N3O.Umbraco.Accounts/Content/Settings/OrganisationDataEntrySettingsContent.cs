using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Content;

public class OrganisationDataEntrySettingsContent : UmbracoContent<OrganisationDataEntrySettingsContent> {
    public string OrganisationName => GetValue(x => x.OrganisationName);
    public string CharityRegistration => GetValue(x => x.CharityRegistration);
    public string AddressSingleLine => GetValue(x => x.AddressSingleLine);
    public string AddressPostalCode => GetValue(x => x.AddressPostalCode);
    public Country AddressCountry => GetValue(x => x.AddressCountry);
}