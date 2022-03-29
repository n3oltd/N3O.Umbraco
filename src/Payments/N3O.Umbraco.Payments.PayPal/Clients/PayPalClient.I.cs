using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Clients {
    public interface IPayPalClient {
        [Post("/v2/payments/authorizations/{req.authorizationId}/capture")]
        Task<ApiAuthorizePaymentRes> AuthorizePaymentAsync(ApiAuthorizePaymentReq req);
    }
}