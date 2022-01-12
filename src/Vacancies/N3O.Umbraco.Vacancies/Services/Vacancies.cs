using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Vacancies.Content;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Vacancies {
    public class Vacancies : IVacancies {
        private readonly ILocalClock _localClock;
        private readonly IContentCache _contentCache;

        public Vacancies(ILocalClock localClock, IContentCache contentCache) {
            _localClock = localClock;
            _contentCache = contentCache;
        }
    
        public IReadOnlyList<T> GetOpen<T>() where T : IPublishedContent {
            var today = _localClock.GetLocalToday().ToDateTimeUnspecified();

            var all = _contentCache.All<T>();
            var open = all.Where(x => x.As<Vacancy>().ClosingDate >= today).ToList();

            return open;
        }
    }
}