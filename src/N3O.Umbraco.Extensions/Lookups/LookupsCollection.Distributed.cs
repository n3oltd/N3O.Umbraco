using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups {
    [StaticLookups]
    public abstract class DistributedLookupsCollection<T, TInterface> : LookupsCollection<T> where T : ILookup {
        private static readonly IReadOnlyList<T> All;

        static DistributedLookupsCollection() {
            var classes = OurAssemblies.GetTypes(t => t.IsConcreteClass() && t.ImplementsInterface<TInterface>());
            
            All = classes.SelectMany(StaticLookups.GetAll<T>).ToList();
        }

        public override Task<IReadOnlyList<T>> GetAllAsync() {
            return Task.FromResult(All);
        }
    }
}
