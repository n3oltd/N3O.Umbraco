using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Vacancies {
    public interface IVacancies {
        IReadOnlyList<T> GetOpen<T>() where T : IPublishedContent;
    }
}