using N3O.Umbraco.Blog.Content;
using N3O.Umbraco.Blog.Criteria;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blog.QueryFilters;

public class BlogPostQueryFilter : ContentQueryFilter<BlogPostCriteria> {
    public override IEnumerable<T> Apply<T>(IEnumerable<T> q, BlogPostCriteria criteria) {
        q = FilterCategory(q, criteria);
        q = FilterDate(q, criteria);

        return q;
    }

    private IEnumerable<T> FilterCategory<T>(IEnumerable<T> q, BlogPostCriteria criteria)
        where T : IPublishedContent {
        if (criteria.Category.HasAny()) {
            q = Where<T, BlogPostContent>(q, x => x.Categories.ContainsAny(criteria.Category));
        }

        return q;
    }

    private IEnumerable<T> FilterDate<T>(IEnumerable<T> q, BlogPostCriteria criteria) where T : IPublishedContent {
        if (criteria.Date.HasValue()) {
            if (criteria.Date.HasFromValue()) {
                q = Where<T, BlogPostContent>(q, x => x.Date >= criteria.Date.From.GetValueOrThrow());
            }
            
            if (criteria.Date.HasToValue()) {
                q = Where<T, BlogPostContent>(q, x => x.Date <= criteria.Date.To.GetValueOrThrow());
            }
        }

        return q;
    }
}
