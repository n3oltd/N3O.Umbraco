using NodaTime;
using System;

namespace N3O.Umbraco.Entities {
    public interface IEntity {
        Guid Id { get; }
        public Instant Timestamp { get; }
        public int Revision { get; }

        void OnSaving(Instant timestamp);
    }
}