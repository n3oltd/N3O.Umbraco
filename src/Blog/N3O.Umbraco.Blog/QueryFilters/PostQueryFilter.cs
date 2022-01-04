using N3O.Umbraco.Blog.Content;
using N3O.Umbraco.Blog.Criteria;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Blog.QueryFilters {
    public class PostQueryFilter : IQueryFilter<Post, PostCriteria> {
        public IEnumerable<Post> Apply(PostCriteria criteria, IEnumerable<Post> q) {
            if (criteria.Date.HasValue()) {
                if (criteria.Date.HasFromValue()) {
                    q = q.Where(x => x.Date >= criteria.Date.From.GetValueOrThrow());
                }
                
                if (criteria.Date.HasToValue()) {
                    q = q.Where(x => x.Date <= criteria.Date.To.GetValueOrThrow());
                }
            }

            if (criteria.Category.HasAny()) {
                q = q.Where(x => criteria.Category.Contains(x.Category));
            }

            return q;
        }
    }
}