using N3O.Umbraco.Events.Content;
using N3O.Umbraco.Events.Criteria;
using N3O.Umbraco.Events.QueryFilters;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Events {
    public class Events : IEvents {
        private readonly IContentCache _contentCache;
        private readonly EventQueryFilter _postQueryFilter;

        public Events(IContentCache contentCache, EventQueryFilter postQueryFilter) {
            _contentCache = contentCache;
            _postQueryFilter = postQueryFilter;
        }

        public IReadOnlyList<T> FindEvents<T>(EventCriteria criteria) where T : IPublishedContent {
            var all = _contentCache.All<T>().Select(x => x.As<Event>()).ToList();

            var results = _postQueryFilter.Apply(criteria, all);

            return results.Select(x => x.Content).Cast<T>().ToList();
        }
    }
}