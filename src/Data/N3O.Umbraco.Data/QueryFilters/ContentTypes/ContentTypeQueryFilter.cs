using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.QueryFilters;

public class ContentTypeQueryFilter : QueryFilter<IContentType, ContentTypeCriteria> {
    public override IEnumerable<IContentType> Apply(IEnumerable<IContentType> q, ContentTypeCriteria criteria) {
        q = FilterAlias(q, criteria);

        return q;
    }

    private IEnumerable<IContentType> FilterAlias(IEnumerable<IContentType> q, ContentTypeCriteria criteria) {
        if (criteria.Alias.HasValue()) {
            q = q.Where(x => x.Alias.EqualsInvariant(criteria.Alias));
        }

        return q;
    }
}
