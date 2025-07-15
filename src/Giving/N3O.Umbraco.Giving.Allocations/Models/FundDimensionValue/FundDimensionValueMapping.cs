using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public abstract class FundDimensionValueMapping<T> : IMapDefinition where T : IFundDimensionValue {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<T, FundDimensionValueRes>((_, _) => new FundDimensionValueRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(T src, FundDimensionValueRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

        dest.IsUnrestricted = src.IsUnrestricted;
    }
}

public class FundDimension1ValueMapping : FundDimensionValueMapping<FundDimension1Value> { }
public class FundDimension2ValueMapping : FundDimensionValueMapping<FundDimension1Value> { }
public class FundDimension3ValueMapping : FundDimensionValueMapping<FundDimension1Value> { }
public class FundDimension4ValueMapping : FundDimensionValueMapping<FundDimension1Value> { }
