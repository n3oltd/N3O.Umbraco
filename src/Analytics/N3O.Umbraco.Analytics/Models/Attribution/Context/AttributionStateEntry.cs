using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Analytics.Models;

public class AttributionStateEntry : Value {
    public int Index { get; set; }
    public string Option { get; set; }
    public Instant Expires { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Index;
        yield return Option;
        yield return Expires;
    }
}