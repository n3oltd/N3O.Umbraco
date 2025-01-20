using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Elements.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class BrandingSettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<OrganisationDataEntrySettingsContent, BrandingSettings>((_, _) => new BrandingSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(OrganisationDataEntrySettingsContent src, BrandingSettings dest, MapperContext ctx) {
        dest.CharityRegistration = src.CharityRegistration;
        dest.OrganisationName = src.OrganisationName;
        dest.AddressSingleLine = src.AddressSingleLine;
    }
}