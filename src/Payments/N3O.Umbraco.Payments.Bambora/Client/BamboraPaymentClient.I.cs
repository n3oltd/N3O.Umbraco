using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public interface IBamboraPaymentClient {
        [Post("/payments")]
        Task<ApiPaymentRes> CreatePaymentAsync(ApiPaymentReq req);
        
        [Post("/payments/{req.ThreeDSessionData}/continue")]
        Task<ApiPaymentRes> CompleteThreeDSecureAsync(ThreeDSecureChallenge req);
    }
}