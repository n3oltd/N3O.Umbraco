using System.Threading.Tasks;

namespace N3O.Umbraco.Entities {
    public interface IRepository<T> where T : IEntity {
        Task DeleteAsync(T entity);
        Task<T> GetAsync(EntityId id);
        Task<T> GetAsync(RevisionId revisionId);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity, RevisionBehaviour revisionBehaviour = RevisionBehaviour.Increment);
    }
}