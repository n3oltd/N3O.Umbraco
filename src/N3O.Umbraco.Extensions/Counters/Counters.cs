using N3O.Umbraco.Entities;
using N3O.Umbraco.Locks;
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
            var id = key.ToGuid();

            var result = await _lock.LockAsync(lockKey, async () => {
                var counter = await _repository.GetAsync(id) ?? await InitializeCounterAsync(key, startFrom);

                var next = counter.Next;
                
                counter.Increment();

                await _repository.UpdateAsync(counter);

                return next;
            });

            return result;
        }

        private async Task<Counter> InitializeCounterAsync(string key, long startFrom) {
            var counter = new Counter();
            counter.Initialize(startFrom);

            await _repository.InsertAsync(counter);

            return counter;
        }
    }
}