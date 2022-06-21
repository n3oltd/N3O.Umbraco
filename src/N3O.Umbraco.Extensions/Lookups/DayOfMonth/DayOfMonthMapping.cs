using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Lookups;

public class DayOfMonthMapping : IMapDefinition {
    private readonly INumberFormatter _numberFormatter;

    public DayOfMonthMapping(INumberFormatter numberFormatter) {
        _numberFormatter = numberFormatter;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DayOfMonth, NamedLookupRes>((_, _) => new NamedLookupRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(DayOfMonth src, NamedLookupRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);
        
        dest.Name = src.ToOrdinal(_numberFormatter);
    }
}
