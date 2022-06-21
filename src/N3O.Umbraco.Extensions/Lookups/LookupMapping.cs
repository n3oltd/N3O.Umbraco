using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Lookups;

public class LookupMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ILookup, LookupRes>((_, _) => new LookupRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ILookup src, LookupRes dest, MapperContext ctx) {
        dest.Id = src.Id;
    }
}
