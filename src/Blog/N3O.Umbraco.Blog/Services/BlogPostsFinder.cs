using N3O.Umbraco.Blog.Content;
using N3O.Umbraco.Blog.Criteria;
using N3O.Umbraco.Blog.QueryFilters;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blog {
    public class BlogPostsFinder : IBlogPostsFinder {
        private readonly IContentCache _contentCache;
        private readonly BlogPostQueryFilter _queryFilter;

        public BlogPostsFinder(IContentCache contentCache, BlogPostQueryFilter queryFilter) {
            _contentCache = contentCache;
            _queryFilter = queryFilter;
        }

        public IReadOnlyList<T> FindPosts<T>(BlogPostCriteria criteria) where T : IPublishedContent {
            var all = _contentCache.All<T>().Select(x => x.As<BlogPost>()).ToList();

            var results = _queryFilter.Apply(criteria, all);

            return results.Select(x => x.Content).Cast<T>().ToList();
        }
    }
}