using Humanizer;
using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Lookups {
    public abstract class  LookupContent<T> : UmbracoContent<T>, INamedLookup {
        public virtual string Id => GetId();

        public virtual string Name => Content.Name;

        protected override IEnumerable<object> GetAtomicValues() {
            yield return Id;
        }
        
        private string GetId() {
            var name = Content.Name;

            if (name == name.ToUpperInvariant()) {
                name = name.ToLowerInvariant();
            }
            
            return name.Camelize();
        }
    }
}