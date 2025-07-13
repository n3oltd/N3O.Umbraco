using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FundDimensionOptionsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IFundDimensionOptions, FundDimensionOptionsRes>((_, _) => new FundDimensionOptionsRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IFundDimensionOptions src, FundDimensionOptionsRes dest, MapperContext ctx) {
        dest.Dimension1 = src.Dimension1;
        dest.Dimension2 = src.Dimension2;
        dest.Dimension3 = src.Dimension3;
        dest.Dimension4 = src.Dimension4;
    }
}
