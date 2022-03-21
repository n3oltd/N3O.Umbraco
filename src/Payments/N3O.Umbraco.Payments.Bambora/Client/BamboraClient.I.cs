using N3O.Umbraco.Payments.Models;
using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public interface IBamboraClient {
        [Post("/Payment")]
        Task<PaymentRes> CreatePaymentAsync(ApiPaymentReq req);
    }
}