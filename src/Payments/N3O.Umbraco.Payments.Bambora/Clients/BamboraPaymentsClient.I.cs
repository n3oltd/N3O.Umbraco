using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public interface IBamboraPaymentsClient {
    [Post("/payments")]
    Task<ApiPaymentRes> CreatePaymentAsync(ApiPaymentReq req);
    
    [Post("/payments/{req.ThreeDSessionData}/continue")]
    Task<ApiPaymentRes> CompleteThreeDSecureAsync(ThreeDSecureChallenge req);
}
