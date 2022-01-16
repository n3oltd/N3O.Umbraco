using N3O.Umbraco.Vacancies.Criteria;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Vacancies {
    public interface IVacanciesFinder {
        IReadOnlyList<T> FindVacancies<T>(VacancyCriteria criteria) where T : IPublishedContent;
        IReadOnlyList<T> FindOpenVacancies<T>(VacancyCriteria criteria = null) where T : IPublishedContent;
    }
}