using N3O.Umbraco.Analytics.Context;
using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.Analytics;

public class AttributionAccessor : IAttributionAccessor {
    private readonly AttributionCookie _attributionCookie;

    public AttributionAccessor(AttributionCookie attributionCookie) {
        _attributionCookie = attributionCookie;
    }

    public JObject GetAttribution() {
        return _attributionCookie.GetAttribution();
    }
}