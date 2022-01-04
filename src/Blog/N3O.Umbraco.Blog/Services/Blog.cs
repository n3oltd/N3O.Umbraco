using N3O.Umbraco.Blog.Content;
using N3O.Umbraco.Blog.Criteria;
using N3O.Umbraco.Blog.QueryFilters;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blog {
    public class Blog : IBlog {
        private readonly IContentCache _contentCache;
        private readonly PostQueryFilter _postQueryFilter;

        public Blog(IContentCache contentCache, PostQueryFilter postQueryFilter) {
            _contentCache = contentCache;
            _postQueryFilter = postQueryFilter;
        }

        public IReadOnlyList<T> FindPosts<T>(PostCriteria criteria) where T : PublishedContentModel {
            var all = _contentCache.All<T>().As<Post>();

            var results = _postQueryFilter.Apply(criteria, all);

            return results.Cast<T>().ToList();
        }
    }
}