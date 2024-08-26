using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models;

public class IndividualMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Individual, IndividualRes>((_, _) => new IndividualRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Individual src, IndividualRes dest, MapperContext ctx) {
        dest.Name = ctx.Map<Name, NameRes>(src.Name);
    }
}