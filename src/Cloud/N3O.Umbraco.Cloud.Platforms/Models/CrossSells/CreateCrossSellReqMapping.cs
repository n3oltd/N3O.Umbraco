using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CreateCrossSellReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CrossSellContent, CreateCrossSellReq>((_, _) => new CreateCrossSellReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(CrossSellContent src, CreateCrossSellReq dest, MapperContext ctx) {
        dest.Name = src.Name;
    }
}