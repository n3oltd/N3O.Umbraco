using N3O.Umbraco.Extensions;
using NodaTime;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Analytics.Models;

public class AttributionEvents : Value {
    public AttributionEvents(IEnumerable<AttributionEvent> events) {
        Events = events.OrEmpty().ToList();
    }

    public IEnumerable<AttributionEvent> Events { get; }

    public AttributionEvents PurgeExpired(IClock clock) {
        var now = clock.GetCurrentInstant();
        var events = Events.Where(x => x.Expires > now);

        return new AttributionEvents(events);
    }

    protected override IEnumerable<object> GetAtomicValues() {
        foreach (var e in Events) {
            yield return e;
        }
    }

    public static AttributionEvents From(params IEnumerable<AttributionEvent>[] attributionEvents) {
        return new AttributionEvents(attributionEvents.SelectMany(x => x));
    }
    
    public static readonly AttributionEvents Empty = new(null);
}