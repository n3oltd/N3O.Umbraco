using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public abstract class FundDimensionMapping<T, TOption> : IMapDefinition where T : FundDimension<T, TOption> {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<FundDimension<T, TOption>, FundDimensionRes>((_, _) => new FundDimensionRes(), Map);
        }

        // Umbraco.Code.MapAll -Id -Name
        private void Map(FundDimension<T, TOption> src, FundDimensionRes dest, MapperContext ctx) {
            ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

            dest.IsActive = src.IsActive;
            dest.Options = src.Options
                              .OrEmpty()
                              .Select(ctx.Map<TOption, FundDimensionOptionRes>)
                              .ToList();
        }
    }
    
    public class FundDimension1Mapping : FundDimensionMapping<FundDimension1, FundDimension1Option> { }
    public class FundDimension2Mapping : FundDimensionMapping<FundDimension2, FundDimension2Option> { }
    public class FundDimension3Mapping : FundDimensionMapping<FundDimension3, FundDimension3Option> { }
    public class FundDimension4Mapping : FundDimensionMapping<FundDimension4, FundDimension4Option> { }
}