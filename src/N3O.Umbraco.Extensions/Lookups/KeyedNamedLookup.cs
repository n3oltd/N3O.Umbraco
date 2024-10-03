namespace N3O.Umbraco.Lookups;

public class KeyedNamedLookup : NamedLookup, IKeyedNamedLookup {
    public KeyedNamedLookup(string id, string name, uint key) : base(id, name) {
        Key = key;
    }

    public KeyedNamedLookup(INamedLookup namedLookup, uint key) : this(namedLookup.Id, namedLookup.Name, key) { }
    
    public KeyedNamedLookup(IKeyedNamedLookup keyedNamedLookup) : this(keyedNamedLookup, keyedNamedLookup.Key) { }
    
    public uint Key { get; }
}