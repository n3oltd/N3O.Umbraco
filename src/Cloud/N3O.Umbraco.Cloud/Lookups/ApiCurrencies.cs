using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiCurrencies : ApiLookupsCollection<Currency> {
    private readonly ICdnClient _cdnClient;

    public ApiCurrencies(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<Currency>> FetchAsync(CancellationToken cancellationToken) {
        var publishedCurrencies = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCurrencies>(SubscriptionFiles.Currencies,
                                                                                                         JsonSerializers.Simple,
                                                                                                         cancellationToken);
        
        var currencies = new List<Currency>();

        foreach (var publishedCurrency in publishedCurrencies.Currencies) {
            var currency = new Currency(publishedCurrency.Code.ToLowerInvariant(),
                                        publishedCurrency.Name,
                                        publishedCurrency.Code,
                                        publishedCurrency.Symbol,
                                        publishedCurrency.DecimalDigits,
                                        publishedCurrency.IsBaseCurrency);
            
            currencies.Add(currency);
        }

        return currencies;
    }

    public override async Task<Currency> FindByIdAsync(string id, CancellationToken cancellationToken = default) {
        var currency = await base.FindByIdAsync(id, cancellationToken);

        if (currency == null && id?.Length > 3) {
            currency = await base.FindByIdAsync(id.Substring(0, 3), cancellationToken);
        }

        return currency;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(12);
}