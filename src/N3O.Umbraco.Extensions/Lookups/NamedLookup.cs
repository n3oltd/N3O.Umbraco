using System.Collections.Generic;

namespace N3O.Umbraco.Lookups {
    public abstract class NamedLookup : Lookup, INamedLookup {
        protected NamedLookup(string id, string name) : base(id) {
            Name = name;
        }

        public virtual string Name { get; }
        
        public override IEnumerable<string> GetTextValues() {
            foreach (var value in base.GetTextValues()) {
                yield return value;
            }

            yield return Name;
        }
    
        public override string ToString() {
            return Name;
        }
    }
}
