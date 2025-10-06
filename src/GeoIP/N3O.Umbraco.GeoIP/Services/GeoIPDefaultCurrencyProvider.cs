using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.GeoIP;

public class GeoIPDefaultCurrencyProvider : LookupsDefaultCurrencyProvider {
    private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
    private readonly IIPGeoLookupCache _ipGeoLookupCache;

    public GeoIPDefaultCurrencyProvider(ILookups lookups,
                                        IRemoteIpAddressAccessor remoteIpAddressAccessor,
                                        IIPGeoLookupCache ipGeoLookupCache)
        : base(lookups) {
        _remoteIpAddressAccessor = remoteIpAddressAccessor;
        _ipGeoLookupCache = ipGeoLookupCache;
    }

    public override async Task<Currency> GetDefaultCurrencyAsync(CancellationToken cancellationToken = default) {
        var remoteIp = _remoteIpAddressAccessor.GetRemoteIpAddress();

        if (remoteIp == null) {
            return await base.GetDefaultCurrencyAsync(cancellationToken);
        }
        
        var geoLookupResult = await _ipGeoLookupCache.GeoLocateIpAsync(remoteIp, cancellationToken);

        var currency = default(Currency);
        
        if (geoLookupResult.Success) {
            currency = geoLookupResult.Country.GetCurrency(AllCurrencies);
        }

        if (currency == null) {
            currency = await base.GetDefaultCurrencyAsync(cancellationToken);
        }

        return currency;
    }
}