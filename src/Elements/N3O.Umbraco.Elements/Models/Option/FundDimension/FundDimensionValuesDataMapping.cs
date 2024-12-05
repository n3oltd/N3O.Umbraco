using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class FundDimensionValuesDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IFundDimensionValues, FundDimensionValuesData>((_, _) => new FundDimensionValuesData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IFundDimensionValues src, FundDimensionValuesData dest, MapperContext ctx) {
        dest.Dimension1 = src.Dimension1;
        dest.Dimension2 = src.Dimension2;
        dest.Dimension3 = src.Dimension3;
        dest.Dimension4 = src.Dimension4;
    }
}
