using N3O.Umbraco.Constants;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using NodaTime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Entities;

public class Repository<T> : IRepository<T> where T : class, IEntity {
    private readonly ConcurrentDictionary<EntityId, T> _entityStore = new();
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IChangeFeedFactory _changeFeedFactory;
    private readonly IJsonProvider _jsonProvider;
    private readonly IClock _clock;

    public Repository(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                      IChangeFeedFactory changeFeedFactory,
                      IJsonProvider jsonProvider,
                      IClock clock) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _changeFeedFactory = changeFeedFactory;
        _jsonProvider = jsonProvider;
        _clock = clock;
    }

    public async Task DeleteAsync(T entity) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            await db.ExecuteAsync($"DELETE FROM {Tables.Entities.Name} WHERE Id = '{entity.Id.Value}'");

            await RunChangeFeedsAsync(EntityOperations.Delete,
                                      null,
                                      entity);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var rows = await db.FetchAsync<EntityRow>($"SELECT * FROM {Tables.Entities.Name} WHERE Type = '{typeof(T).AssemblyQualifiedName}'");

            var entities = new List<T>();

            foreach (var row in rows) {
                var type = Type.GetType(row.Type);

                _entityStore[row.Id] = (T) _jsonProvider.DeserializeObject(row.Json, type);

                entities.Add((T) _jsonProvider.DeserializeObject(row.Json, type));
            }

            return entities;
        }
    }

    public async Task<T> GetAsync(EntityId id, CancellationToken cancellationToken = default) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var row = await db.SingleOrDefaultAsync<EntityRow>($"SELECT * FROM {Tables.Entities.Name} WHERE Id = '{id.Value}'");

            var entity = row.IfNotNull(x => {
                var type = Type.GetType(x.Type);
                
                _entityStore[id] = (T) _jsonProvider.DeserializeObject(x.Json, type);
                
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

    public async Task InsertAsync(T entity) {
        await SaveAsync(entity, (db, r) => {
            db.Insert(Tables.Entities.Name, Tables.Entities.PrimaryKey, false, r);
            
            return Task.CompletedTask;
        });
        
        await RunChangeFeedsAsync(EntityOperations.Insert, entity, null);
    }
    
    public async Task UpdateAsync(T entity, RevisionBehaviour revisionBehaviour = RevisionBehaviour.Increment) {
        // TODO The update SQL should check the revision number hasn't changed and fail if it has
        await SaveAsync(entity, (db, r) => db.UpdateAsync(r), revisionBehaviour);
        
        await RunChangeFeedsAsync(EntityOperations.Update, entity, _entityStore.GetOrDefault(entity.Id));
    }

    private async Task SaveAsync(T entity,
                                 Func<IUmbracoDatabase, EntityRow, Task> saveAsync,
                                 RevisionBehaviour revisionBehaviour = RevisionBehaviour.Increment) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            entity.OnSaving(_clock.GetCurrentInstant(), revisionBehaviour);

            var row = new EntityRow();
            row.Id = entity.Id;
            row.Revision = entity.Revision;
            row.Timestamp = entity.Timestamp.ToDateTimeUtc();
            row.Type = entity.GetType().AssemblyQualifiedName;
            row.Json = _jsonProvider.SerializeObject(entity);

            await saveAsync(db, row);
        }
    }
        
    private async Task RunChangeFeedsAsync(EntityOperation operation,
                                           T sessionEntity,
                                           T dbEntity) {
        var entityType = (sessionEntity ?? dbEntity).GetType();
        
        var changeFeeds = _changeFeedFactory.GetChangeFeeds(entityType);

        foreach (var changeFeed in changeFeeds) {
            var entityChange = new EntityChange(sessionEntity, dbEntity, operation);

            await changeFeed.ProcessChangeAsync(entityChange);
        }
    }
}
