using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class PriceDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPrice, PriceData>((_, _) => new PriceData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IPrice src, PriceData dest, MapperContext ctx) {
        dest.Amount = src.Amount;
        dest.Locked = src.Locked;
    }
}
