using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups {
    public abstract class StaticLookupsCollection<T> : LookupsCollection<T> where T : ILookup {
        private readonly IReadOnlyList<T> _all;

        protected StaticLookupsCollection() {
            _all = StaticLookups.GetAll<T>(GetType());
        }

        public override Task<IReadOnlyList<T>> GetAllAsync() {
            return Task.FromResult(_all);
        }
    }
}