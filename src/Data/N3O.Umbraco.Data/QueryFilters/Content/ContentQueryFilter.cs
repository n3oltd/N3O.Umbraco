using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Data.QueryFilters;

public class ContentQueryFilter : QueryFilter<IPublishedContent, ContentCriteria> {
    public override IEnumerable<IPublishedContent> Apply(IEnumerable<IPublishedContent> q, ContentCriteria criteria) {
        q = FilterAlias(q, criteria);

        return q;
    }

    private IEnumerable<IPublishedContent> FilterAlias(IEnumerable<IPublishedContent> q, ContentCriteria criteria) {
        if (criteria.ContentTypeAlias.HasValue()) {
            q = q.Where(x => x.ContentType.Alias.EqualsInvariant(criteria.ContentTypeAlias));
        }

        return q;
    }
}
