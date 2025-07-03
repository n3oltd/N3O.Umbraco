using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public interface ILookupsCollection {
    Task<ILookup> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ILookup>> FindByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ILookup>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface ILookupsCollection<T> : ILookupsCollection where T : ILookup {
    new Task<T> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    new Task<IEnumerable<T>> FindByNameAsync(string name, CancellationToken cancellationToken = default);
    new Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
}
