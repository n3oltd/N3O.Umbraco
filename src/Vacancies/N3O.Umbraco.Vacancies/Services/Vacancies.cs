using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Vacancies.Content;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Vacancies; 

public class Vacancies : IVacancies {
    private readonly ILocalClock _localClock;
    private readonly IContentLocator _contentLocator;

    public Vacancies(ILocalClock localClock, IContentLocator contentLocator) {
        _localClock = localClock;
        _contentLocator = contentLocator;
    }
    
    public IReadOnlyList<T> GetOpen<T>() where T : PublishedContentModel {
        var today = _localClock.GetLocalToday().ToDateTimeUnspecified();

        var all = _contentLocator.All<T>();
        var open = all.Where(x => x.As<Vacancy>().ClosingDate >= today).ToList();

        return open;
    }
}