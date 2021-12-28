using Humanizer;
using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Lookups;

public abstract class LookupContent : UmbracoContent, INamedLookup {
    public virtual string Id => Content.Name.Pascalize();
    public virtual string Name => Content.Name;

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
    }
}