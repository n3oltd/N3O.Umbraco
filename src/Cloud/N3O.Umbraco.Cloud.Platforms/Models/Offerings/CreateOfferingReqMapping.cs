using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CreateOfferingReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<OfferingContent, CreateOfferingReq>((_, _) => new CreateOfferingReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(OfferingContent src, CreateOfferingReq dest, MapperContext ctx) {
        dest.Name = src.Name;
    }
}