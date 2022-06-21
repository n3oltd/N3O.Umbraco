using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.QueryFilters;

public interface IContentQueryFilter<in TCriteria> {
    IEnumerable<T> Apply<T>(IEnumerable<T> q, TCriteria criteria) where T : IPublishedContent;
}
