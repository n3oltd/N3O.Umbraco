using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedOrganizationInfoMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;

    public PublishedOrganizationInfoMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
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
        dest.Logo = _mediaUrl.GetMediaUrl(src.Logo, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
    }
}