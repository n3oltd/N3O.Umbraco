namespace N3O.Umbraco.Lookups;

public interface INamedLookup : ILookup {
    string Name { get; }
}
