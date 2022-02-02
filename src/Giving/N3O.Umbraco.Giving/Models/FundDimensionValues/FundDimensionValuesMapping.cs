using Umbraco.Cms.Core.Mapping;

namespace N3O.Giving.Models {
    public class FundDimensionValuesMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<IFundDimensionValues, FundDimensionValuesRes>((_, _) => new FundDimensionValuesRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(IFundDimensionValues src, FundDimensionValuesRes dest, MapperContext ctx) {
            dest.Dimension1 = src.Dimension1;
            dest.Dimension2 = src.Dimension2;
            dest.Dimension3 = src.Dimension3;
            dest.Dimension4 = src.Dimension4;
        }
    }
}