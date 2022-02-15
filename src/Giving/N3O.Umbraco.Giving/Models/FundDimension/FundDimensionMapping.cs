using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Giving.Models {
    public abstract class FundDimensionMapping<T, TValue> : IMapDefinition where T : FundDimension<T, TValue> {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<FundDimension<T, TValue>, FundDimensionRes>((_, _) => new FundDimensionRes(), Map);
        }

        // Umbraco.Code.MapAll -Id -Name
        private void Map(FundDimension<T, TValue> src, FundDimensionRes dest, MapperContext ctx) {
            ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

            dest.Index = src.Index;
            dest.IsActive = src.IsActive;
            dest.Options = src.Options
                              .OrEmpty()
                              .Select(ctx.Map<TValue, FundDimensionValueRes>)
                              .ToList();
        }
    }
    
    public class FundDimension1Mapping : FundDimensionMapping<FundDimension1, FundDimension1Value> { }
    public class FundDimension2Mapping : FundDimensionMapping<FundDimension2, FundDimension2Value> { }
    public class FundDimension3Mapping : FundDimensionMapping<FundDimension3, FundDimension3Value> { }
    public class FundDimension4Mapping : FundDimensionMapping<FundDimension4, FundDimension4Value> { }
}