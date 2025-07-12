using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.GeoIP.Models;

public class GeoLookupResult : Value {
    public GeoLookupResult(bool success, Country country, string city) {
        Success = success;
        Country = country;
        City = city;
    }

    public bool Success { get; }
    public Country Country { get; }
    public string City { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Success;
        yield return Country;
        yield return City;
    }

    public static GeoLookupResult ForFailure() {
        return new GeoLookupResult(false, null, null);
    }
    
    public static GeoLookupResult ForSuccess(Country country, string city) {
        return new GeoLookupResult(true, country, city);
    }
}
