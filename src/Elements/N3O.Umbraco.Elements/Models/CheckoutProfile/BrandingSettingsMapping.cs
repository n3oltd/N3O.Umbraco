using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Utilities;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class BrandingSettingsMapping : IMapDefinition {
    private readonly IUrlBuilder _urlBuilder;

    public BrandingSettingsMapping(IUrlBuilder urlBuilder) {
        _urlBuilder = urlBuilder;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<OrganisationDataEntrySettingsContent, BrandingSettings>((_, _) => new BrandingSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(OrganisationDataEntrySettingsContent src, BrandingSettings dest, MapperContext ctx) {
        dest.CharityRegistration = src.CharityRegistration;
        dest.OrganisationName = src.OrganisationName;
        dest.AddressSingleLine = src.AddressSingleLine;
        dest.AddressPostalCode = src.AddressPostalCode;
        dest.AddressCountry = (Country) Enum.Parse(typeof(Country), src.AddressCountry.Id, true);
        dest.LogoUrl = _urlBuilder.Root().AppendPathSegment(src.Logo.Src);
    }
}