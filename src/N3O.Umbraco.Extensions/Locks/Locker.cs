using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Locks {
    // Based on https://stackoverflow.com/a/31194647
    public class Locker : ILocker {
        private static readonly Dictionary<string, RefCounted<SemaphoreSlim>> SemaphoreSlims = new();

        public IDisposable Lock(string key) {
            GetOrCreate(key).Wait();
            
            return new Releaser { Key = key };
        }

        public async Task<IDisposable> LockAsync(string key) {
            await GetOrCreate(key).WaitAsync();
            
            return new Releaser { Key = key };
        }

        private SemaphoreSlim GetOrCreate(string name) {
            RefCounted<SemaphoreSlim> item;
            
            lock (SemaphoreSlims) {
                if (SemaphoreSlims.TryGetValue(name, out item)) {
                    item.RefCount++;
                } else {
                    item = new RefCounted<SemaphoreSlim>(new SemaphoreSlim(1, 1));
                    
                    SemaphoreSlims[name] = item;
                }
            }
            
            return item.Value;
        }

        private class RefCounted<T> {
            public RefCounted(T value) {
                RefCount = 1;
                Value = value;
            }

            public int RefCount { get; set; }
            public T Value { get; }
        }

        private class Releaser : IDisposable {
            public string Key { get; set; }

            public void Dispose() {
                RefCounted<SemaphoreSlim> item;

                lock (SemaphoreSlims) {
                    item = SemaphoreSlims[Key];
                    item.RefCount--;

                    if (item.RefCount == 0) {
                        SemaphoreSlims.Remove(Key);
                    }
                }

                item.Value.Release();
            }
        }
    }
}