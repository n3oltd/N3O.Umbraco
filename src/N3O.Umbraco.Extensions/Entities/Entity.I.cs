using NodaTime;
using System;

namespace N3O.Umbraco.Entities {
    public interface IEntity {
        Guid Id { get; }
        Instant Timestamp { get; }
        int Revision { get; }
        bool IsNew { get; }

        void OnSaving(Instant timestamp);
    }
}