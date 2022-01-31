using N3O.Umbraco.Constants;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Scoping;

namespace N3O.Umbraco.Entities {
    public class Repository<T> : IRepository<T> where T : class, IEntity {
        private readonly IScopeProvider _scopeProvider;
        private readonly IJsonProvider _jsonProvider;
        private readonly IClock _clock;

        public Repository(IScopeProvider scopeProvider, IJsonProvider jsonProvider, IClock clock) {
            _scopeProvider = scopeProvider;
            _jsonProvider = jsonProvider;
            _clock = clock;
        }

        public Task DeleteAsync(EntityId id, CancellationToken cancellationToken = default) {
            using (var scope = _scopeProvider.CreateScope()) {
                scope.Database.Delete<EntityRow>($@"DELETE FROM {Tables.Entities} WHERE Id = @0", id.Value);
            
                scope.Complete();

                return Task.CompletedTask;
            }
        }
        
        public async Task<T> GetAsync(EntityId id, CancellationToken cancellationToken = default) {
            using (var scope = _scopeProvider.CreateScope()) {
                var row = await scope.Database.SingleOrDefaultAsync<EntityRow>($@"SELECT * FROM {Tables.Entities} WHERE Id = @0", id.Value);

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
            await SaveAsync(entity, (s, r) => s.Database.Insert(r));
        }
        
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default) {
            // TODO The update SQL should check the revision number hasn't changed and fail if it has
            await SaveAsync(entity, (s, r) => s.Database.Update(r));
        }

        private Task<TResult> SaveAsync<TResult>(T entity, Func<IScope, EntityRow, TResult> save) {
            using (var scope = _scopeProvider.CreateScope()) {
                entity.OnSaving(_clock.GetCurrentInstant());

                var row = new EntityRow();
                row.Id = entity.Id;
                row.Revision = entity.Revision;
                row.Timestamp = entity.Timestamp.ToDateTimeUtc();
                row.Type = entity.GetType().AssemblyQualifiedName;
                row.Json = _jsonProvider.SerializeObject(entity);

                var result = save(scope, row);
            
                scope.Complete();

                return Task.FromResult(result);
            }
        }
    }
}