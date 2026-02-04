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
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
    private readonly IFormatter _formatter;
    private readonly INisab _nisab;

    public NisabMergeModelProvider(INisab nisab, IBaseCurrencyAccessor baseCurrencyAccessor, IFormatter formatter) {
        _baseCurrencyAccessor = baseCurrencyAccessor;
        _formatter = formatter;
        _nisab = nisab;
    }

    protected override async Task PopulateModelsAsync(IPublishedContent content,
                                                      Dictionary<string, object> mergeModels,
                                                      CancellationToken cancellationToken = default) {
        var baseCurrency = _baseCurrencyAccessor.GetBaseCurrency();
        
        var goldNisabAmount = await _nisab.GetGoldNisabAsync(baseCurrency, cancellationToken);
        var goldNisab = _formatter.Number.FormatMoney(goldNisabAmount);
        var silverNisabAmount = await _nisab.GetSilverNisabAsync(baseCurrency, cancellationToken);
        var silverNisab = _formatter.Number.FormatMoney(silverNisabAmount);

        mergeModels[PlatformsTemplateConstants.ModelKeys.Nisab] = new NisabMergeModel(goldNisab, silverNisab);
    }
}