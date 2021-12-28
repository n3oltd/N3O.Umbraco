namespace N3O.Umbraco.Lookups;

public abstract class NamedLookup : Lookup, INamedLookup {
    protected NamedLookup(string id, string name) : base(id) {
        Name = name;
    }

    public string Name { get; }
    
    public override string ToString() {
        return Name;
    }
}
