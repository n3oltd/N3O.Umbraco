using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups {
    public interface ILookups {
        Task<T> FindByIdAsync<T>(string id, CancellationToken cancellationToken = default) where T : ILookup;
        T FindById<T>(string id) where T : ILookup;

        Task<ILookup> FindByIdAsync(Type lookupType, string id, CancellationToken cancellationToken = default);
        ILookup FindById(Type lookupType, string id);

        Task<IReadOnlyList<T>> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : ILookup;
        IReadOnlyList<T> GetAll<T>() where T : ILookup;

        Task<IReadOnlyList<ILookup>> GetAllAsync(Type lookupType, CancellationToken cancellationToken = default);
        IReadOnlyList<ILookup> GetAll(Type lookupType);
    }
}
