using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class GivingSchedule : ContentOrPublishedLookup {
    public GivingSchedule(string id, string name, Guid? contentId) : base(id, name, contentId) { }
}