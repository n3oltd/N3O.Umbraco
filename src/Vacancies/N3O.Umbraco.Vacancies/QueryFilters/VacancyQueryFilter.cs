using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using N3O.Umbraco.Vacancies.Content;
using N3O.Umbraco.Vacancies.Criteria;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Vacancies.QueryFilters {
    public class VacancyQueryFilter : IQueryFilter<Vacancy, VacancyCriteria> {
        public IEnumerable<Vacancy> Apply(VacancyCriteria criteria, IEnumerable<Vacancy> q) {
            q = FilterClosingDate(criteria, q);

            return q;
        }

        private IEnumerable<Vacancy> FilterClosingDate(VacancyCriteria criteria, IEnumerable<Vacancy> q) {
            if (criteria.ClosingDate.HasValue()) {
                if (criteria.ClosingDate.HasFromValue()) {
                    q = q.Where(x => x.ClosingDate >= criteria.ClosingDate.From.GetValueOrThrow());
                }
                
                if (criteria.ClosingDate.HasToValue()) {
                    q = q.Where(x => x.ClosingDate <= criteria.ClosingDate.To.GetValueOrThrow());
                }
            }

            return q;
        }
    }
}