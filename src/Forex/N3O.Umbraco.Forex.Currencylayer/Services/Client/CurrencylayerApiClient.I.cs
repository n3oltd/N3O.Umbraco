using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex.Currencylayer;

public interface ICurrencylayerApiClient {
    [Get("/live")]
    Task<ApiResponse> GetLiveRateAsync(LiveRateRequest request);
}
