using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoAddressEntryReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<AddressEntryContent, UmbracoAddressEntryReq>((_, _) => new UmbracoAddressEntryReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(AddressEntryContent src, UmbracoAddressEntryReq dest, MapperContext ctx) { ;
        dest.GoogleMapsApiKey = src.GoogleMapsApiKey;
    }
}