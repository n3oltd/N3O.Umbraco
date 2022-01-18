using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public abstract class FundDimensionOptionMapping<T> : IMapDefinition where T : FundDimensionOption<T> {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<FundDimensionOption<T>, FundDimensionOptionRes>((_, _) => new FundDimensionOptionRes(), Map);
        }

        // Umbraco.Code.MapAll -Id -Name
        private void Map(FundDimensionOption<T> src, FundDimensionOptionRes dest, MapperContext ctx) {
            ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

            dest.IsUnrestricted = src.IsUnrestricted;
        }
    }
    
    public class FundDimension1OptionMapping : FundDimensionOptionMapping<FundDimension1Option> { }
    public class FundDimension2OptionMapping : FundDimensionOptionMapping<FundDimension2Option> { }
    public class FundDimension3OptionMapping : FundDimensionOptionMapping<FundDimension3Option> { }
    public class FundDimension4OptionMapping : FundDimensionOptionMapping<FundDimension4Option> { }
}