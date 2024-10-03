namespace N3O.Umbraco.Lookups;

public interface IKeyedLookup : ILookup {
    uint Key { get; }
}