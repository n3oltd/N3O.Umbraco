using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Extensions {
    public static class ConcurrentDictionaryExtensions {
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> Semaphores = new();

        public static TValue GetOrAddAtomic<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict,
                                                          TKey key,
                                                          Func<TValue> factory) {
            var result = GetOrAddAtomicAsync(dict, key, () => Task.FromResult(factory())).GetAwaiter().GetResult();

            return result;
        }


        public static async Task<TValue> GetOrAddAtomicAsync<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict,
                                                                           TKey key,
                                                                           Func<Task<TValue>> asyncFactory) {
            if (dict.TryGetValue(key, out var value)) {
                return value;
            }

            var isOwner = false;
            var semaphoreKey = (dict, key).GetHashCode();

            if (!Semaphores.TryGetValue(semaphoreKey, out var semaphore)) {
                SemaphoreSlim createdSemaphore = null;

                Semaphores.GetOrAdd(semaphoreKey, _ => createdSemaphore = new SemaphoreSlim(1));
                semaphore = Semaphores[semaphoreKey];

                if (createdSemaphore != semaphore) {
                    createdSemaphore?.Dispose();
                } else {
                    isOwner = true;
                }
            }

            await semaphore.WaitAsync();

            try {
                if (!dict.TryGetValue(key, out value)) {
                    value = await asyncFactory();

                    dict[key] = value;
                }

                return value;
            } finally {
                if (isOwner) {
                    Semaphores.TryRemove(semaphoreKey, out _);
                }

                semaphore.Release();
            }
        }
        
        public static TValue TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> source, TKey key) {
            if (source.TryRemove(key, out var result)) {
                return result;
            }

            return default;
        }
    }
}
