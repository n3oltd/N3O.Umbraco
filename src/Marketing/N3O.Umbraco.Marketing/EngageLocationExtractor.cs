using N3O.Umbraco.Extensions;
using N3O.Umbraco.GeoIP;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Engage.Data.Analytics.Collection.Pageview;
using Umbraco.Engage.Infrastructure.Analytics.Processed;
using Umbraco.Engage.Infrastructure.Analytics.Processing.Extractors;

namespace N3O.Umbraco.Marketing;

// Which provider locates a visitor depends on how a site is fronted, so a site references one and this
// package knows only the abstraction. Taken as a collection because a site referencing none is an ordinary
// state: it still analyses pageviews, just without a location on them. Depending on the provider directly
// makes that state fail the container's startup validation instead, which takes the whole site down
public class EngageLocationExtractor : IRawPageviewLocationExtractor {
    private static readonly int MaxColumnWidth = 100;

    private readonly IIPGeoLocationProvider _ipGeoLocationProvider;

    public EngageLocationExtractor(IEnumerable<IIPGeoLocationProvider> ipGeoLocationProviders) {
        // Ordered rather than taken as registered, so a site referencing more than one gets whichever the
        // providers themselves declare precedence for instead of whichever happened to register last
        _ipGeoLocationProvider = ipGeoLocationProviders.ApplyAttributeOrdering().FirstOrDefault();
    }

    public ILocation Extract(IRawPageview rawPageview) {
        if (_ipGeoLocationProvider == null) {
            return null;
        }

        var geoLookupResult = _ipGeoLocationProvider.GeoLocateAsync().GetAwaiter().GetResult();

        if (!geoLookupResult.Success) {
            return null;
        }

        var location = new EngageLocation();
        location.City = WithinColumnWidth(geoLookupResult.City);
        location.Country = geoLookupResult.Country?.Name;
        location.County = Location.Unknown.County;
        location.Province = WithinColumnWidth(geoLookupResult.Province);

        return location;
    }

    // Engage stores city and province as nvarchar(100), and a longer value faults its processing pipeline.
    private static string WithinColumnWidth(string value) {
        return value?.Length > MaxColumnWidth ? null : value;
    }
}
