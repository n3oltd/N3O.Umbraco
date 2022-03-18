using N3O.Umbraco.Payments.PayPal.Client.Models;
using Refit;
using System.Threading.Tasks;


namespace N3O.Umbraco.Payments.PayPal.Client {
    public interface IPayPalClient {
        [Post("/v2/payments/authorizations/{req.authorizationId}/capture")]
        Task<AuthorizePaymentRes> AuthorizePaymentAsync(AuthorizePaymentReq req);
    }
}