using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Financial;

public class CurrencyMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Currency, CurrencyRes>((_, _) => new CurrencyRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(Currency src, CurrencyRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

        dest.Code = src.Code;
        dest.Symbol = src.Symbol;
        dest.Icon = src.Icon.AbsoluteUri;
        dest.DecimalDigits = src.DecimalDigits;
        dest.IsBaseCurrency = src.IsBaseCurrency;
        
    }
}
