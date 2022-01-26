using N3O.Umbraco.Entities;
using N3O.Umbraco.Locks;
using N3O.Umbraco.Utilities;
using System;
using System.Threading;
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
        
        public async Task<long> NextAsync(string key,
                                          long startFrom = 1,
                                          CancellationToken cancellationToken = default) {
            var lockKey = LockKey.Generate<Counters>(key);

            var result = await _lock.LockAsync(lockKey, async () => {
                var id = key.ToGuid();
                
                var counter = await _repository.GetAsync(id, cancellationToken) ??
                              await CreateCounterAsync(id, startFrom, cancellationToken);

                var next = counter.Next;
                
                counter.Increment();

                await _repository.UpdateAsync(counter, cancellationToken);

                return next;
            });

            return result;
        }

        public async Task<Reference> NextAsync(ReferenceType referenceType,
                                               CancellationToken cancellationToken = default) {
            var number = await NextAsync(referenceType.Id, referenceType.StartFrom, cancellationToken);

            return new Reference(referenceType, number);
        }

        public async Task<Reference> NextAsync<TReferenceType>(CancellationToken cancellationToken = default)
            where TReferenceType : ReferenceType, new() {
            var referenceType = new TReferenceType();
            var reference = await NextAsync(referenceType, cancellationToken);

            return reference;
        }

        private async Task<Counter> CreateCounterAsync(Guid id, long startFrom, CancellationToken cancellationToken) {
            var counter = Counter.Create(id, startFrom);

            await _repository.InsertAsync(counter, cancellationToken);

            return counter;
        }
    }
}