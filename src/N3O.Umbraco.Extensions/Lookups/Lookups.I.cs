using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public interface ILookups {
    Task<T> FindByIdAsync<T>(string id) where T : ILookup;
    T FindById<T>(string id) where T : ILookup;

    Task<ILookup> FindByIdAsync(Type lookupType, string id);
    ILookup FindById(Type lookupType, string id);

    Task<IReadOnlyList<T>> GetAllAsync<T>() where T : ILookup;
    IReadOnlyList<T> GetAll<T>() where T : ILookup;

    Task<IReadOnlyList<ILookup>> GetAllAsync(Type lookupType);
    IReadOnlyList<ILookup> GetAll(Type lookupType);
}
