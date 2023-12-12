using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Lookups;

public abstract class  LookupContent<T> : UmbracoContent<T>, INamedLookup {
    public virtual string Id { get; set; }

    public virtual string Name => Content().Name;

    public IEnumerable<string> GetTextValues() {
        yield return Id;
        yield return Name;
    }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
    }
}
