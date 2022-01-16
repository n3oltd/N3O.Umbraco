using N3O.Umbraco.Blog.Content;
using N3O.Umbraco.Blog.Criteria;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Blog.QueryFilters {
    public class BlogPostQueryFilter : IQueryFilter<BlogPost, BlogPostCriteria> {
        public IEnumerable<BlogPost> Apply(BlogPostCriteria criteria, IEnumerable<BlogPost> q) {
            q = FilterCategory(criteria, q);
            q = FilterDate(criteria, q);

            return q;
        }

        private IEnumerable<BlogPost> FilterCategory(BlogPostCriteria criteria, IEnumerable<BlogPost> q) {
            if (criteria.Category.HasAny()) {
                q = q.Where(x => x.Categories.ContainsAny(criteria.Category));
            }

            return q;
        }

        private IEnumerable<BlogPost> FilterDate(BlogPostCriteria criteria, IEnumerable<BlogPost> q) {
            if (criteria.Date.HasValue()) {
                if (criteria.Date.HasFromValue()) {
                    q = q.Where(x => x.Date >= criteria.Date.From.GetValueOrThrow());
                }
                
                if (criteria.Date.HasToValue()) {
                    q = q.Where(x => x.Date <= criteria.Date.To.GetValueOrThrow());
                }
            }

            return q;
        }
    }
}