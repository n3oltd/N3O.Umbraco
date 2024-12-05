using J2N.Collections.Generic;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PriceHandleMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PriceHandleElement, PriceHandleRes>((_, _) => new PriceHandleRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PriceHandleElement src, PriceHandleRes dest, MapperContext ctx) {
        dest.Amount = src.Amount;
        dest.CurrencyValues = ctx.Map<decimal, Dictionary<string, MoneyRes>>(src.Amount);
        dest.Description = src.Description;
    }
}
