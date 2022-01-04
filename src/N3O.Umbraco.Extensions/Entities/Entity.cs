using NodaTime;
using System;

namespace N3O.Umbraco.Entities {
    public abstract class Entity : IEntity {
        protected Entity() {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; private set; }
        public Instant Timestamp { get; private set; }
        public int Revision { get; private set; }
        
        public void OnSaving(Instant timestamp) {
            Timestamp = timestamp;
            Revision++;
        }

        public static TEntity Create<TEntity>(Guid? id = null) where TEntity : Entity, new() {
            var entity = new TEntity();

            if (id.HasValue) {
                entity.Id = id.Value;
            }

            return entity;
        }
    }
}