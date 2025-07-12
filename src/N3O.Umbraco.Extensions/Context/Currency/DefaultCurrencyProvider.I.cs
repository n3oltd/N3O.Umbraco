using N3O.Umbraco.Financial;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Context;

public interface IDefaultCurrencyProvider {
    Task<Currency> GetDefaultCurrencyAsync(CancellationToken cancellationToken = default);
}