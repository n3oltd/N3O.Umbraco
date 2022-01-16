using N3O.Umbraco.Content;
using N3O.Umbraco.Vacancies.Content;
using N3O.Umbraco.Vacancies.Criteria;
using N3O.Umbraco.Vacancies.QueryFilters;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Vacancies {
    public class VacanciesFinder : IVacanciesFinder {
        private readonly IContentCache _contentCache;
        private readonly ILocalClock _localClock;
        private readonly VacancyQueryFilter _queryFilter;

        public VacanciesFinder(IContentCache contentCache, ILocalClock localClock, VacancyQueryFilter queryFilter) {
            _contentCache = contentCache;
            _localClock = localClock;
            _queryFilter = queryFilter;
        }

        public IReadOnlyList<T> FindVacancies<T>(VacancyCriteria criteria) where T : IPublishedContent {
            var all = _contentCache.All<T>().Select(x => x.As<Vacancy>()).ToList();

            var results = _queryFilter.Apply(criteria, all);

            return results.Select(x => x.Content).Cast<T>().ToList();
        }
        
        public IReadOnlyList<T> FindOpenVacancies<T>(VacancyCriteria criteria = null) where T : IPublishedContent {
            criteria ??= new VacancyCriteria();

            if (criteria.ClosingDate.HasValue()) {
                throw new Exception($"Criteria passed to {nameof(FindOpenVacancies)} cannot specify {nameof(VacancyCriteria.ClosingDate)}");
            }
            
            criteria.ClosingDate = new Range<LocalDate?>(null, _localClock.GetLocalToday());

            return FindVacancies<T>(criteria);
        }
    }
}