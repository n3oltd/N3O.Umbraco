namespace N3O.Umbraco.Lookups;

public class KeyedLookup : Lookup, IKeyedLookup {
    public KeyedLookup(string id, uint key) : base(id) {
        Key = key;
    }

    public KeyedLookup(ILookup lookup, uint key) : this(lookup.Id, key) { }
    
    public KeyedLookup(IKeyedLookup keyedLookup) : this(keyedLookup, keyedLookup.Key) { }
    
    public uint Key { get; }
}