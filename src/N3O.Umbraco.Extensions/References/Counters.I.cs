using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.References {
    public interface ICounters {
        Task<long> NextAsync(string key, long startFrom = 1, CancellationToken cancellationToken = default);
        Task<Reference> NextAsync(ReferenceType referenceType, CancellationToken cancellationToken = default);
        Task<Reference> NextAsync<TReferenceType>(CancellationToken cancellationToken = default)
            where TReferenceType : ReferenceType, new();
    }
}