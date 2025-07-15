using System;

namespace N3O.Umbraco.Lookups;

public abstract class ContentOrPublishedLookup : NamedLookup, IContentOrPublishedLookup {
    protected ContentOrPublishedLookup(string id, string name, Guid? contentId) : base(id, name) {
        ContentId = contentId;
    }

    public Guid? ContentId { get; }
}
