using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Locks {
    public class Lock : ILock {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> LocksDictionary = new();

        public async Task LockAsync(string name, Func<Task> action) {
            var semaphoreSlim = GetSemaphoreSlim(name);

            await semaphoreSlim.WaitAsync();

            try {
                await action();
            } finally {
                semaphoreSlim.Release();
            }
        }

        public async Task<T> LockAsync<T>(string name, Func<Task<T>> action) {
            var semaphoreSlim = GetSemaphoreSlim(name);

            await semaphoreSlim.WaitAsync();

            try {
                return await action();
            } finally {
                semaphoreSlim.Release();
            }
        }

        private SemaphoreSlim GetSemaphoreSlim(string name) {
            var semaphoreSlim = LocksDictionary.GetOrAdd(name, new SemaphoreSlim(1, 1));

            return semaphoreSlim;
        }
    }
}