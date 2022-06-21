using System;

namespace N3O.Umbraco.Content {
    public interface IContentEditor {
        IContentPublisher ForExisting(Guid id);
        IContentPublisher New(string name, Guid parentId, string contentTypeAlias, Guid? contentKey = null);
    }
}