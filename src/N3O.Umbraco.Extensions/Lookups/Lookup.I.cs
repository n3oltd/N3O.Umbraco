using System.Collections.Generic;

namespace N3O.Umbraco.Lookups;

public interface ILookup {
    string Id { get; }
    string Name { get; }
    IEnumerable<string> GetTextValues();
}
