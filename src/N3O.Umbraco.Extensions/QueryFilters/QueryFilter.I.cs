using System.Collections.Generic;

namespace N3O.Umbraco.QueryFilters;

public interface IQueryFilter<T, in TCriteria> {
    IEnumerable<T> Apply(IEnumerable<T> q, TCriteria criteria);
}
