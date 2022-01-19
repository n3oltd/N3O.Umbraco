using N3O.Umbraco.Blog.Criteria;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blog {
    public interface IBlogPostsFinder {
        IReadOnlyList<T> FindPosts<T>(BlogPostCriteria criteria = null) where T : IPublishedContent;
    }
}