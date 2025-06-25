using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedOrganizationInfoMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PublishedOrganizationInfoMapping(IContentLocator contentLocator, IWebHostEnvironment webHostEnvironment) {
        _contentLocator = contentLocator;
        _webHostEnvironment = webHostEnvironment;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<OrganizationInfoContent, PublishedOrganizationInfo>((_, _) => new PublishedOrganizationInfo(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(OrganizationInfoContent src, PublishedOrganizationInfo dest, MapperContext ctx) {
        dest.Name = src.OrganisationName;
        dest.AddressSingleLine = src.AddressSingleLine;
        dest.AddressPostalCode = src.AddressPostalCode;
        dest.AddressCountry = src.AddressCountry.ToEnum<Country>();
        dest.CharityRegistration = src.CharityRegistration;
        dest.Logo = src.Logo.GetPublishedUri(_contentLocator, _webHostEnvironment);
    }
}