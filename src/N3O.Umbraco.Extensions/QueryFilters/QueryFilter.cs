using System.Collections.Generic;

namespace N3O.Umbraco.QueryFilters {
    public abstract class QueryFilter<T, TCriteria> : IQueryFilter<T, TCriteria> {
        public abstract IEnumerable<T> Apply(IEnumerable<T> q, TCriteria criteria);
    }
}