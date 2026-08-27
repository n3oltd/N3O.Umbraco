using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Net;
using Umbraco.Engage.Data.Analytics.Collection.Pageview;
using Umbraco.Engage.Infrastructure.Analytics.Processed;
using Umbraco.Engage.Infrastructure.Analytics.Processing.Extractors;
using Umbraco.Extensions;

namespace N3O.Umbraco.Marketing;

public class EngageLocationExtractor : IRawPageviewLocationExtractor {
    private static readonly string CityHeader = "cf-ipcity";
    private static readonly string CountryHeader = "CF-IPCountry";
    private static readonly string RegionHeader = "cf-region";

    private readonly ILookups _lookups;

    public EngageLocationExtractor(ILookups lookups) {
        _lookups = lookups;
    }

    public ILocation Extract(IRawPageview rawPageview) {
        if (!IPAddress.TryParse(rawPageview?.IpAddress, out var ipAddress) || IPAddress.IsLoopback(ipAddress)) {
            return null;
        }

        var headers = rawPageview.Headers;

        if (headers == null) {
            return null;
        }

        // Cloudflare only populates these beyond CF-IPCountry when the visitor location headers managed transform
        // is enabled, and reports XX for an unknown country and T1 for Tor, neither of which is a country code.
        var countryCode = headers.GetValue(CountryHeader);
        var country = _lookups.GetAll<Country>().FindByCode(countryCode);

        var location = new EngageLocation();
        location.City = Decode(headers.GetValue(CityHeader));
        location.Country = country?.Name;
        location.Province = Decode(headers.GetValue(RegionHeader));

        return location;
    }

    // Cloudflare percent-encodes location header values that are not plain ASCII.
    private string Decode(string value) {
        return value.HasValue() ? WebUtility.UrlDecode(value) : null;
    }
}
