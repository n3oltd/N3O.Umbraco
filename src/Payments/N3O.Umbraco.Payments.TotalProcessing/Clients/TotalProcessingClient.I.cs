using Payments.TotalProcessing.Clients.Models;
using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Clients;

public interface ITotalProcessingClient {
    [Post("/v1/checkouts")]
    public Task<TokenRes> PrepareCheckoutAsync(TokenReq req);

    [Post("/v1/checkouts")]
    public Task<TokenRes> PrepareCheckoutAsync(PaymentReq req);

    [Get("/v1/checkouts/{paymentId}/payment")]
    public Task<ApiTransactionRes> GetPaymentAsync([Query] string entityId, string paymentId);
}
