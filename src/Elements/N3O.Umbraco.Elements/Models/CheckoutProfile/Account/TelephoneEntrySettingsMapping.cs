using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Elements.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class TelephoneEntrySettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PhoneDataEntrySettings, TelephoneEntrySettings>((_, _) => new TelephoneEntrySettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PhoneDataEntrySettings src, TelephoneEntrySettings dest, MapperContext ctx) {
        dest.Required = src.Number.Required;
        dest.Validate = src.Number.Validate;
    }
}