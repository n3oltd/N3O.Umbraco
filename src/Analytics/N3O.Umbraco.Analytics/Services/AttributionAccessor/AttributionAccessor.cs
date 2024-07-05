using N3O.Umbraco.Analytics.Context;
using N3O.Umbraco.Analytics.Models;

namespace N3O.Umbraco.Analytics.Services;

public class AttributionAccessor : IAttributionAccessor {
    
    private readonly AttributionEventsCookie _attributionEventsCookie;

    public AttributionAccessor(AttributionEventsCookie attributionEventsCookie) {
        _attributionEventsCookie = attributionEventsCookie;
    }

    public Attribution GetAttribution() {
        return _attributionEventsCookie.GetAttribution();
    }
}