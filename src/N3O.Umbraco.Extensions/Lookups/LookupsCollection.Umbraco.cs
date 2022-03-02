using N3O.Umbraco.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups {
    public class UmbracoLookupsCollection<T> : LookupsCollection<T> where T : LookupContent<T> {
        private readonly IContentCache _contentCache;

        public UmbracoLookupsCollection(IContentCache contentCache) {
            _contentCache = contentCache;
        }

        public override Task<IReadOnlyList<T>> GetAllAsync() {
            var items = _contentCache.All<T>().OrderBy(x => x.Content().SortOrder).ToList();

            return Task.FromResult<IReadOnlyList<T>>(items);
        }
    }
}
