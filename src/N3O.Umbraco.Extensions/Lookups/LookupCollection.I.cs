using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public interface ILookupsCollection {
    Task<ILookup> FindByIdAsync(string id);
    public Task<ILookup> FindByNameAsync(string name);
    Task<IReadOnlyList<ILookup>> GetAllAsync();
}

public interface ILookupsCollection<T> : ILookupsCollection where T : ILookup {
    new Task<T> FindByIdAsync(string id);
    new Task<T> FindByNameAsync(string name);
    new Task<IReadOnlyList<T>> GetAllAsync();
}
