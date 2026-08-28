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
    private static readonly int MaxColumnWidth = 100;

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

        var countryCode = headers.GetValue(CountryHeader);
        var city = headers.GetValue(CityHeader);
        var region = headers.GetValue(RegionHeader);

        if (!countryCode.HasValue() && !city.HasValue() && !region.HasValue()) {
            return null;
        }

        var location = new EngageLocation();
        location.City = WithinColumnWidth(city);
        location.Country = countryCode.HasValue() ? _lookups.GetAll<Country>().FindByCode(countryCode)?.Name : null;
        // Cloudflare carries no second-level subdivision, so this component is never resolvable.
        location.County = Location.Unknown.County;
        location.Province = WithinColumnWidth(region);

        return location;
    }

    // Engage stores city and province as nvarchar(100), and a longer value faults its processing pipeline.
    private static string WithinColumnWidth(string value) {
        return value?.Length > MaxColumnWidth ? null : value;
    }
}
