using System.Collections.Generic;

namespace N3O.Umbraco.Analytics.Models;

public class AttributionState : Value {
    public IEnumerable<AttributionStateEntry> Entries { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        foreach (var entry in Entries) {
            yield return entry;
        }
    }
}