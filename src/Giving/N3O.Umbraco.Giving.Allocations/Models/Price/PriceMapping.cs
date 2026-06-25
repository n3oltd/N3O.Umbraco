using N3O.Umbraco.Financial;
using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PriceMapping : IMapDefinition {
    private readonly ICurrencyRounder _currencyRounder;

    public PriceMapping(ICurrencyRounder currencyRounder) {
        _currencyRounder = currencyRounder;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPrice, PriceRes>((_, _) => new PriceRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IPrice src, PriceRes dest, MapperContext ctx) {
        dest.Amount = src.Amount;
        dest.CurrencyValues = ctx.Map<(decimal, ICurrencyRounder), Dictionary<string, MoneyRes>>((src.Amount, _currencyRounder));
        dest.Locked = src.Locked;
    }
}
