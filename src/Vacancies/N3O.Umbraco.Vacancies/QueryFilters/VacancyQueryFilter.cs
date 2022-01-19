using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using N3O.Umbraco.Vacancies.Content;
using N3O.Umbraco.Vacancies.Criteria;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Vacancies.QueryFilters {
    public class VacancyQueryFilter : ContentQueryFilter<VacancyCriteria> {
        public override IEnumerable<T> Apply<T>(IEnumerable<T> q, VacancyCriteria criteria) {
            q = FilterClosingDate(q, criteria);

            return q;
        }

        private IEnumerable<T> FilterClosingDate<T>(IEnumerable<T> q, VacancyCriteria criteria)
            where T : IPublishedContent {
            if (criteria.ClosingDate.HasValue()) {
                if (criteria.ClosingDate.HasFromValue()) {
                    q = Where<T, VacancyContent>(q, x => x.ClosingDate >= criteria.ClosingDate.From.GetValueOrThrow());
                }
                
                if (criteria.ClosingDate.HasToValue()) {
                    q = Where<T, VacancyContent>(q, x => x.ClosingDate <= criteria.ClosingDate.To.GetValueOrThrow());
                }
            }

            return q;
        }
    }
}