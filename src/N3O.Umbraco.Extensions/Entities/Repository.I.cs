using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Entities;

public interface IRepository<T> where T : IEntity {
    Task DeleteAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> GetAsync(EntityId id, CancellationToken cancellationToken = default);
    Task<T> GetAsync(RevisionId revisionId, CancellationToken cancellationToken = default);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity, RevisionBehaviour revisionBehaviour = RevisionBehaviour.Increment);
}
