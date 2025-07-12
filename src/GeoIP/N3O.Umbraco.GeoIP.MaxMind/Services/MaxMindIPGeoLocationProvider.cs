using MaxMind.GeoIP2;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.GeoIP.Models;
using N3O.Umbraco.Lookups;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.GeoIP.MaxMind;

public class MaxMindIPGeoLocationProvider : IIPGeoLocationProvider {
    private readonly ILookups _lookups;
    private readonly WebServiceClient _webServiceClient;

    public MaxMindIPGeoLocationProvider(ILookups lookups, WebServiceClient webServiceClient) {
        _lookups = lookups;
        _webServiceClient = webServiceClient;
    }

    public async Task<GeoLookupResult> GeoLocateIpAsync(IPAddress ipAddress,
                                                        CancellationToken cancellationToken = default) {
        try {
            var cityResponse = await _webServiceClient.CityAsync(ipAddress);

            var country = _lookups.GetAll<Country>().FindByCode(cityResponse.Country.IsoCode);
                
            return GeoLookupResult.ForSuccess(country, cityResponse.City?.Name);
        } catch { }

        return GeoLookupResult.ForFailure();
    }
}
