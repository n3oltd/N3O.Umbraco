using N3O.Umbraco.Entities;
using N3O.Umbraco.Locks;
using N3O.Umbraco.Utilities;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.References;

public class Counters : ICounters {
    private readonly ILocker _locker;
    private readonly IRepository<Counter> _repository;

    public Counters(ILocker locker, IRepository<Counter> repository) {
        _locker = locker;
        _repository = repository;
    }
    
    public async Task<long> NextAsync(string key,
                                      long startFrom = 1,
                                      CancellationToken cancellationToken = default) {
        var lockKey = LockKey.Generate<Counters>(key);

        using (await _locker.LockAsync(lockKey)) {
            var id = key.ToGuid();
            
            var counter = await _repository.GetAsync(id) ?? await CreateCounterAsync(id, startFrom);

            var next = counter.Next;
            
            counter.Increment();

            await _repository.UpdateAsync(counter);

            return next;
        }
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

    private async Task<Counter> CreateCounterAsync(EntityId id, long startFrom) {
        var counter = Counter.Create(id, startFrom);

        await _repository.InsertAsync(counter);

        return counter;
    }
}
