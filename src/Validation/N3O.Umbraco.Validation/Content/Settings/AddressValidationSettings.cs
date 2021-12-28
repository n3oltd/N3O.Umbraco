using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Validation.Content;

public class AddressValidationSettings : UmbracoContent {
    public Country DefaultCountry => GetValue<AddressValidationSettings, Country>(x => x.DefaultCountry);
}
