using N3O.Umbraco.Entities;
using N3O.Umbraco.Locks;
using System;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Counters {
    public class Counters : ICounters {
        private readonly ILock _lock;
        private readonly IRepository<Counter> _repository;

        public Counters(ILock @lock, IRepository<Counter> repository) {
            _lock = @lock;
            _repository = repository;
        }
        
        public async Task<long> NextAsync(string key, long startFrom = 1) {
            var lockKey = $"{nameof(Counters)}_{key}";

            var result = await _lock.LockAsync(lockKey, async () => {
                var id = key.ToGuid();
                
                var counter = await _repository.GetAsync(id) ?? await InitializeCounterAsync(id, startFrom);

                var next = counter.Next;
                
                counter.Increment();

                await _repository.UpdateAsync(counter);

                return next;
            });

            return result;
        }

        private async Task<Counter> InitializeCounterAsync(Guid id, long startFrom) {
            var counter = Entity.Create<Counter>(id);
            counter.Initialize(startFrom);

            await _repository.InsertAsync(counter);

            return counter;
        }
    }
}