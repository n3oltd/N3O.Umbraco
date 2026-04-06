using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class QurbaniSeason : ContentOrPublishedLookup {
    public QurbaniSeason(string id, string name, Guid? contentId) : base(id, name, contentId) { }
}