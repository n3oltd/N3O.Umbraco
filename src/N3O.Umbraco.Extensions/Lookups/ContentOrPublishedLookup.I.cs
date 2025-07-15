using System;

namespace N3O.Umbraco.Lookups;

public interface IContentOrPublishedLookup : INamedLookup {
    Guid? ContentId { get; }
}
