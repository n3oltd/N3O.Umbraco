using N3O.Umbraco.Blog.Criteria;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blog {
    public interface IBlog {
        IReadOnlyList<T> FindPosts<T>(PostCriteria criteria) where T : PublishedContentModel;
    }
}