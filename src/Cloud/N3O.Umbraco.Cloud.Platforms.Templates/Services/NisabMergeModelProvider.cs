using N3O.Umbraco.Cloud.Platforms.Templates.Models;
using N3O.Umbraco.Context;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public class NisabMergeModelProvider : MergeModelsProvider {
    private readonly ICurrencyAccessor _currencyAccessor;
    private readonly IFormatter _formatter;
    private readonly INisab _nisab;

    public NisabMergeModelProvider(INisab nisab, ICurrencyAccessor currencyAccessor, IFormatter formatter) {
        _currencyAccessor = currencyAccessor;
        _formatter = formatter;
        _nisab = nisab;
    }

    protected override async Task PopulateModelsAsync(IPublishedContent content,
                                                      Dictionary<string, object> mergeModels,
                                                      CancellationToken cancellationToken = default) {
        var currency = _currencyAccessor.GetCurrency();
        
        var goldNisabAmount = await _nisab.GetGoldNisabAsync(currency, cancellationToken);
        var goldNisab = _formatter.Number.FormatMoney(goldNisabAmount);
        var silverNisabAmount = await _nisab.GetSilverNisabAsync(currency, cancellationToken);
        var silverNisab = _formatter.Number.FormatMoney(silverNisabAmount);

        mergeModels["nisab"] = new NisabMergeModel(goldNisab, silverNisab);
    }
}