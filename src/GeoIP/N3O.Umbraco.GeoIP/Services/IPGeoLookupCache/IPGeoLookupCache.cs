using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.GeoIP.Models;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.GeoIP;

public class IPGeoLookupCache : IIPGeoLookupCache {
    private static readonly MemoryCache ResultsCache = new(new MemoryCacheOptions());
    private readonly IIPGeoLocationProvider _ipGeoLocationProvider;

    public IPGeoLookupCache(IIPGeoLocationProvider ipGeoLocationProvider) {
        _ipGeoLocationProvider = ipGeoLocationProvider;
    }

    public async Task<GeoLookupResult> GeoLocateIpAsync(IPAddress ipAddress,
                                                        CancellationToken cancellationToken = default) {
        return await ResultsCache.GetOrCreateAsync(ipAddress, async c => {
            c.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);

            return await _ipGeoLocationProvider.GeoLocateIpAsync(ipAddress, cancellationToken);
        });
    }
}
