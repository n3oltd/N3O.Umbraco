using System.Threading.Tasks;
using Refit;

namespace N3O.Umbraco.Forex.Currencylayer {
    public interface ICurrencylayerApiClient {
        [Get("/live")]
        Task<ApiResponse> GetLiveRateAsync(LiveRateRequest request);

        [Get("/historical")]
        Task<ApiResponse> GetHistoricalRateAsync(HistoricalRateRequest request);
    }
}
