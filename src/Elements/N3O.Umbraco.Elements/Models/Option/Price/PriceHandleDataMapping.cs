using N3O.Umbraco.Giving.Allocations.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class PriceHandleDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PriceHandleElement, PriceHandleData>((_, _) => new PriceHandleData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PriceHandleElement src, PriceHandleData dest, MapperContext ctx) {
        dest.Amount = src.Amount;
        dest.Description = src.Description;
    }
}
