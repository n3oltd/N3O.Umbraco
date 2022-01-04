using N3O.Umbraco.Constants;
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

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) {
            using (var scope = _scopeProvider.CreateScope()) {
                scope.Database.Delete<EntityRow>($@"DELETE FROM {Tables.Entities} WHERE Id = @0", id);
            
                scope.Complete();

                return Task.CompletedTask;
            }
        }
        
        public async Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default) {
            using (var scope = _scopeProvider.CreateScope()) {
                var row = await scope.Database.SingleOrDefaultAsync<EntityRow>($@"SELECT * FROM {Tables.Entities} WHERE Id = @0", id);

                var entity = row.IfNotNull(x => {
                    var type = Type.GetType(x.Type);
                    
                    return (T) _jsonProvider.DeserializeObject(x.Json, type);
                });

                return entity;
            }
        }

        public Task InsertAsync(T entity, CancellationToken cancellationToken = default) {
            return SaveAsync(entity, (s, r) => s.Database.InsertAsync(r));
        }
        
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default) {
            await SaveAsync(entity, (s, r) => s.Database.UpdateAsync(r));
        }

        private async Task SaveAsync(T entity, Func<IScope, EntityRow, Task> saveAsync) {
            using (var scope = _scopeProvider.CreateScope()) {
                entity.OnSaving(_clock.GetCurrentInstant());

                var row = new EntityRow();
                row.Id = entity.Id;
                row.Revision = entity.Revision;
                row.Timestamp = entity.Timestamp.ToDateTimeUtc();
                row.Type = entity.GetType().FullName;
                row.Json = _jsonProvider.SerializeObject(entity);

                await saveAsync(scope, row);
            
                scope.Complete();
            }
        }
    }
}