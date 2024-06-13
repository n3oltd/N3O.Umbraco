using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Financial;

public class MoneyMapping : IMapDefinition {
    private readonly IFormatter _formatter;

    public MoneyMapping(IFormatter formatter) {
        _formatter = formatter;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Money, MoneyRes>((_, _) => new MoneyRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Money src, MoneyRes dest, MapperContext ctx) {
        dest.Amount = src.Amount.RoundMoney();
        dest.Currency = src.Currency;
        dest.Text = _formatter.Number.FormatMoney(src);
    }
}
