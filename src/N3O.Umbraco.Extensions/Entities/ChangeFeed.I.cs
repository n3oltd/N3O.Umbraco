using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Entities {
    public interface IChangeFeed<T> where T : IEntity {
        Task ProcessChangeAsync(EntityChange<T> entityChange, CancellationToken cancellationToken);
    }
}