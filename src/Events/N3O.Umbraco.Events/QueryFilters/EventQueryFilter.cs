using N3O.Umbraco.Events.Content;
using N3O.Umbraco.Events.Criteria;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Events.QueryFilters {
    public class EventQueryFilter : IQueryFilter<Event, EventCriteria> {
        public IEnumerable<Event> Apply(EventCriteria criteria, IEnumerable<Event> q) {
            if (criteria.Date.HasValue()) {
                if (criteria.Date.HasFromValue()) {
                    q = q.Where(x => x.Date >= criteria.Date.From.GetValueOrThrow());
                }
                
                if (criteria.Date.HasToValue()) {
                    q = q.Where(x => x.Date <= criteria.Date.To.GetValueOrThrow());
                }
            }

            if (criteria.Category.HasAny()) {
                q = q.Where(x => x.Categories.ContainsAny(criteria.Category));
            }

            return q;
        }
    }
}