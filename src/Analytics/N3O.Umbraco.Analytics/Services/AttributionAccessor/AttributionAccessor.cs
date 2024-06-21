using N3O.Umbraco.Analytics.Context;
using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Analytics.Services;

public class AttributionAccessor : IAttributionAccessor {
    private const decimal OneHundredPercent = 100m;
    
    private readonly AttributionEventsCookie _attributionEventsCookie;

    public AttributionAccessor(AttributionEventsCookie attributionEventsCookie) {
        _attributionEventsCookie = attributionEventsCookie;
    }

    public Attribution GetAttribution() {
        var attributionEvents = _attributionEventsCookie.GetEvents();
        var dimensions = GetDimensions(attributionEvents.Events);
        
        return new Attribution(dimensions);
    }

    private IReadOnlyList<AttributionDimension> GetDimensions(IEnumerable<AttributionEvent> events) {
        var dimensions = new List<AttributionDimension>();
       
        foreach (var dimensionGroup in events.GroupBy(x => x.DimensionIndex)) {
            foreach (var optionGroup in dimensionGroup.GroupBy(x => x.Option.ToLowerInvariant())) {
                var percentages = OneHundredPercent.SafeDivide(optionGroup.Count()).ToList();

                var optionPercentages = optionGroup.Select((x, i) => new OptionPercentage(x.Option, percentages[i]));
                
                dimensions.Add(new AttributionDimension(dimensionGroup.Key, optionPercentages));
            }
        }
        
        return dimensions;
    }
}