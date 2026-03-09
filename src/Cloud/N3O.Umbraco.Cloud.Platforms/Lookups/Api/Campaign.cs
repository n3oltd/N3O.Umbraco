using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class Campaign : ContentOrPublishedLookup {
    public Campaign(string id, string name, Guid? contentId) : base(id, name, contentId) { }
}