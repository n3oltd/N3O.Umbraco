using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Financial;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Zakat;

public interface INisab {
    Task<Money> GetGoldNisabAsync(Currency currency, CancellationToken cancellationToken = default);
    Task<Money> GetNisabAsync(Currency currency, NisabType nisabType, CancellationToken cancellationToken = default);
    Task<Money> GetSilverNisabAsync(Currency currency, CancellationToken cancellationToken = default);
}