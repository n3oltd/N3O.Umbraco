using N3O.Umbraco.Payments.Models;
using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public interface IBamboraClient {
        [Post("/payments")]
        Task<ApiPaymentRes> CreatePaymentAsync(ApiPaymentReq req);
    }
}