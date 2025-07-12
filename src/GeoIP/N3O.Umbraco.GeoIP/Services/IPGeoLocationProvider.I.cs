using N3O.Umbraco.GeoIP.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.GeoIP;

public interface IIPGeoLocationProvider {
    Task<GeoLookupResult> GeoLocateIpAsync(IPAddress ipAddress, CancellationToken cancellationToken = default);
}
