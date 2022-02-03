using N3O.Umbraco.Constants;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Entities {
    public class Repository<T> : IRepository<T> where T : class, IEntity {
        private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
        private readonly IJsonProvider _jsonProvider;
        private readonly IClock _clock;

        public Repository(IUmbracoDatabaseFactory umbracoDatabaseFactory, IJsonProvider jsonProvider, IClock clock) {
            _umbracoDatabaseFactory = umbracoDatabaseFactory;
            _jsonProvider = jsonProvider;
            _clock = clock;
        }

        public async Task DeleteAsync(EntityId id, CancellationToken cancellationToken = default) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                await db.ExecuteAsync($@"DELETE FROM {Tables.Entities.Name} WHERE Id = '{id.Value}'");
            }
        }
        
        public async Task<T> GetAsync(EntityId id, CancellationToken cancellationToken = default) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                var row = await db.SingleOrDefaultAsync<EntityRow>($@"SELECT * FROM {Tables.Entities.Name} WHERE Id = '{id.Value}'");

                var entity = row.IfNotNull(x => {
                    var type = Type.GetType(x.Type);
                    
                    return (T) _jsonProvider.DeserializeObject(x.Json, type);
                });

                return entity;
            }
        }

        public async Task<T> GetAsync(RevisionId revisionId, CancellationToken cancellationToken = default) {
            var entity = await GetAsync(revisionId.Id, cancellationToken);
            
            if (entity != null && !revisionId.RevisionMatches(entity.Revision)) {
                throw new RevisionMismatchException(revisionId);
            }

            return entity;
        }

        public async Task InsertAsync(T entity, CancellationToken cancellationToken = default) {
            await SaveAsync(entity, (db, r) => {
                db.Insert(Tables.Entities.Name, Tables.Entities.PrimaryKey, false, r);
                
                return Task.CompletedTask;
            });
        }
        
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default) {
            // TODO The update SQL should check the revision number hasn't changed and fail if it has
            await SaveAsync(entity, (db, r) => db.UpdateAsync(r));
        }

        private async Task SaveAsync(T entity, Func<IUmbracoDatabase, EntityRow, Task> saveAsync) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                entity.OnSaving(_clock.GetCurrentInstant());

                var row = new EntityRow();
                row.Id = entity.Id;
                row.Revision = entity.Revision;
                row.Timestamp = entity.Timestamp.ToDateTimeUtc();
                row.Type = entity.GetType().AssemblyQualifiedName;
                row.Json = _jsonProvider.SerializeObject(entity);

                await saveAsync(db, row);
            }
        }
    }
}