using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Financial;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public class Nisab : INisab {
    private readonly ICdnClient _cdnClient;

    public Nisab(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    public async Task<Money> GetGoldNisabAsync(Currency currency, CancellationToken cancellationToken = default) {
        return await GetNisabAsync(currency, Metals.Gold, cancellationToken);
    }
    
    public async Task<Money> GetNisabAsync(Currency currency,
                                           Metal metal,
                                           CancellationToken cancellationToken = default) {
        var nisab = await _cdnClient.DownloadSubscriptionContentAsync<PublishedNisab>(SubscriptionFiles.Nisab,
                                                                                      cancellationToken);

        if (!nisab.Amounts.ContainsKey(currency.Code)) {
            throw new Exception($"No nisab value found for currency {currency.Code}");
        }

        decimal nisabAmount;

        if (metal == Metals.Gold) {
            nisabAmount = nisab.Amounts[currency.Code].Gold;
        } else if (metal == Metals.Gold) {
            nisabAmount = nisab.Amounts[currency.Code].Silver;
        } else {
            throw UnrecognisedValueException.For(metal);
        }

        return new Money(nisabAmount, currency);
    }
    
    public async Task<Money> GetSilverNisabAsync(Currency currency, CancellationToken cancellationToken = default) {
        return await GetNisabAsync(currency, Metals.Silver, cancellationToken);
    }
}