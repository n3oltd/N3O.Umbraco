using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Analytics.Models;

public class AttributionEvent : Value {
    public AttributionEvent(int dimensionIndex, string option, Instant expires) {
        DimensionIndex = dimensionIndex;
        Option = option;
        Expires = expires;
    }

    public int DimensionIndex { get; }
    public string Option { get; }
    public Instant Expires { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return DimensionIndex;
        yield return Option;
        yield return Expires;
    }
}