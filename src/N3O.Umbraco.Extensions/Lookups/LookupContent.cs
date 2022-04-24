using Humanizer;
using N3O.Umbraco.Content;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Lookups {
    public abstract class  LookupContent<T> : UmbracoContent<T>, INamedLookup {
        public string Id => GetId();

        public virtual string Name => Content().Name;

        public IEnumerable<string> GetTextValues() {
            yield return Id;
            yield return Name;
        }
        
        protected override IEnumerable<object> GetAtomicValues() {
            yield return Id;
        }
        
        protected virtual string GetId() {
            var name = Content().Name;

            if (name == name.ToUpperInvariant()) {
                name = name.ToLowerInvariant();
            }

            name = Regex.Replace(name, "[^0-9a-z-_]", "", RegexOptions.CultureInvariant|RegexOptions.IgnoreCase);
            
            return name.Camelize();
        }
    }
}