using System.Collections.Generic;

namespace N3O.Umbraco.Lookups {
    public interface ILookup {
        string Id { get; }
        IEnumerable<string> GetTextValues();
    }
}
