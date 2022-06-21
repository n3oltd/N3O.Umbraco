using NodaTime;

namespace N3O.Umbraco.Entities;

public interface IEntity {
    EntityId Id { get; }
    Instant Timestamp { get; }
    int Revision { get; }
    bool IsNew { get; }
    RevisionId RevisionId { get; }

    void OnSaving(Instant timestamp, RevisionBehaviour revisionBehaviour);
}
