using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.FundDimensions {
    public class FundStructureMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<FundStructure, FundStructureRes>((_, _) => new FundStructureRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(FundStructure src, FundStructureRes dest, MapperContext ctx) {
            dest.Dimension1 = ctx.Map<FundDimension<FundDimension1, FundDimension1Option>, FundDimensionRes>(src.Dimension1);
            dest.Dimension2 = ctx.Map<FundDimension<FundDimension2, FundDimension2Option>, FundDimensionRes>(src.Dimension2);
            dest.Dimension3 = ctx.Map<FundDimension<FundDimension3, FundDimension3Option>, FundDimensionRes>(src.Dimension3);
            dest.Dimension4 = ctx.Map<FundDimension<FundDimension4, FundDimension4Option>, FundDimensionRes>(src.Dimension4);
        }
    }
}