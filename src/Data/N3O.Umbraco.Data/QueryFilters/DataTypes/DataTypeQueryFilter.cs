using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.QueryFilters;

public class DataTypeQueryFilter : QueryFilter<IDataType, DataTypeCriteria> {
    public override IEnumerable<IDataType> Apply(IEnumerable<IDataType> q, DataTypeCriteria criteria) {
        q = FilterEditorAlias(q, criteria);

        return q;
    }

    private IEnumerable<IDataType> FilterEditorAlias(IEnumerable<IDataType> q, DataTypeCriteria criteria) {
        if (criteria.EditorAlias.HasValue()) {
            q = q.Where(x => x.EditorAlias.EqualsInvariant(criteria.EditorAlias));
        }

        return q;
    }
}
