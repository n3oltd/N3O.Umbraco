using N3O.Umbraco.Content;
using N3O.Umbraco.Events.Content;
using N3O.Umbraco.Events.Criteria;
using N3O.Umbraco.Events.QueryFilters;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Events {
    public class EventsFinder : IEventsFinder {
        private readonly IContentCache _contentCache;
        private readonly ILocalClock _localClock;
        private readonly EventQueryFilter _queryFilter;

        public EventsFinder(IContentCache contentCache, ILocalClock localClock, EventQueryFilter queryFilter) {
            _contentCache = contentCache;
            _localClock = localClock;
            _queryFilter = queryFilter;
        }

        public IReadOnlyList<T> FindEvents<T>(EventCriteria criteria) where T : IPublishedContent {
            var all = _contentCache.All<T>().Select(x => x.As<Event>()).ToList();

            var results = _queryFilter.Apply(criteria, all);

            return results.Select(x => x.Content).Cast<T>().ToList();
        }
        
        public IReadOnlyList<T> FindOpenEvents<T>(EventCriteria criteria = null) where T : IPublishedContent {
            criteria ??= new EventCriteria();

            if (criteria.Date.HasValue()) {
                throw new Exception($"Criteria passed to {nameof(FindOpenEvents)} cannot specify {nameof(EventCriteria.Date)}");
            }
            
            criteria.Date = new Range<LocalDate?>(null, _localClock.GetLocalToday());

            return FindEvents<T>(criteria);
        }
    }
}