using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Forex.Models;

public class CurrencyValuesMapping : IMapDefinition {
    private readonly ILookups _lookups;
    private readonly IForexConverter _forexConverter;

    public CurrencyValuesMapping(ILookups lookups, IForexConverter forexConverter) {
        _lookups = lookups;
        _forexConverter = forexConverter;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<decimal, Dictionary<string, MoneyRes>>((_, _) => new Dictionary<string, MoneyRes>(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(decimal src, Dictionary<string, MoneyRes> dest, MapperContext ctx) {
        var currencies = _lookups.GetAll<Currency>();
        var baseCurrency = currencies.Single(x => x.IsBaseCurrency);
        var otherCurrencies = currencies.Except(baseCurrency).ToList();
        
        dest[baseCurrency.Code] = ctx.Map<Money, MoneyRes>(new Money(src, baseCurrency));

        foreach (var currency in otherCurrencies) {
            var forexMoney = _forexConverter.BaseToQuote().ToCurrency(currency).Convert(src);
            
            dest[currency.Code] = ctx.Map<Money, MoneyRes>(forexMoney.Quote);
        }
    }
}
