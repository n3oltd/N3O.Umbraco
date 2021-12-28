using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups {
    public class Lookups : ILookups {
        private readonly IServiceProvider _services;

        public Lookups(IServiceProvider services) {
            _services = services;
        }

        public async Task<T> FindByIdAsync<T>(string id) where T : ILookup {
            var lookup = await FindByIdAsync(typeof(T), id);

            return (T) lookup;
        }
    
        public async Task<ILookup> FindByIdAsync(Type lookupType, string id) {
            var result = await ExecuteAsync(lookupType,
                                            lookupsCollection => lookupsCollection.FindByIdAsync(id),
                                            null);

            return result;
        }

        public T FindById<T>(string id) where T : ILookup {
            return FindByIdAsync<T>(id).GetAwaiter().GetResult();
        }

        public ILookup FindById(Type lookupType, string id) {
            return FindByIdAsync(lookupType, id).GetAwaiter().GetResult();
        }

        public IReadOnlyList<T> GetAll<T>() where T : ILookup {
            return GetAllAsync<T>().GetAwaiter().GetResult();
        }

        public async Task<IReadOnlyList<ILookup>> GetAllAsync(Type lookupType) {
            var result = await ExecuteAsync(lookupType,
                                            lookupsCollection => lookupsCollection.GetAllAsync(),
                                            new List<ILookup>());

            return result;
        }

        public IReadOnlyList<ILookup> GetAll(Type lookupType) {
            return GetAllAsync(lookupType).GetAwaiter().GetResult();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync<T>() where T : ILookup {
            var all = await GetAllAsync(typeof(T));

            return all.Cast<T>().ToList();
        }
    
        private async Task<TResult> ExecuteAsync<TResult>(Type lookupType,
                                                          Func<ILookupsCollection, Task<TResult>> getResultAsync,
                                                          TResult defaultResult) {
            var lookupsCollectionType = typeof(ILookupsCollection<>).MakeGenericType(lookupType);
            var lookupsCollection = (ILookupsCollection) _services.GetService(lookupsCollectionType);

            TResult result = defaultResult;
        
            if (lookupsCollection != null) {
                result = await getResultAsync(lookupsCollection);
            }

            return result;
        }
    }
}
