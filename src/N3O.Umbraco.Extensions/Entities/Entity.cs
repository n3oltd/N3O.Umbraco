using N3O.Umbraco.Extensions;
using NodaTime;

namespace N3O.Umbraco.Entities {
    public abstract class Entity : IEntity {
        protected Entity() {
            Id = EntityId.New();
            Revision = 0;
        }
        
        public EntityId Id { get; private set; }
        public Instant Timestamp { get; private set; }
        public int Revision { get; private set; }
        public bool IsNew => Revision == 0;
        
        public void OnSaving(Instant timestamp) {
            Timestamp = timestamp;
            Revision++;
        }

        protected static TEntity Create<TEntity>(EntityId id = null) where TEntity : Entity, new() {
            var entity = new TEntity();

            if (id.HasValue()) {
                entity.Id = id;
            }

            return entity;
        }
    }
}