using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public abstract class FundDimensionValueDataMapping<T> : IMapDefinition where T : FundDimensionValue<T> {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<T, FundDimensionValueData>((_, _) => new FundDimensionValueData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(T src, FundDimensionValueData dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.Name = src.Name;
        dest.IsUnrestricted = src.IsUnrestricted;
    }
}

public class FundDimension1ValueMapping : FundDimensionValueDataMapping<FundDimension1Value> { }
public class FundDimension2ValueMapping : FundDimensionValueDataMapping<FundDimension2Value> { }
public class FundDimension3ValueMapping : FundDimensionValueDataMapping<FundDimension3Value> { }
public class FundDimension4ValueMapping : FundDimensionValueDataMapping<FundDimension4Value> { }
