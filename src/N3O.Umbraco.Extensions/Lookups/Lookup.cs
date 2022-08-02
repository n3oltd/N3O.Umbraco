using System.Collections.Generic;

namespace N3O.Umbraco.Lookups;

public abstract class Lookup : Value, ILookup {
    protected Lookup(string id) {
        Id = id;
    }

    public override string ToString() {
        return Id;
    }

    public string Id { get; }
    public string Name { get; }

    public virtual IEnumerable<string> GetTextValues() {
        yield return Id;
    }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
    }
}
