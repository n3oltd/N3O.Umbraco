using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using Payments.TotalProcessing.Clients.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing;

public interface ITotalProcessingHelper {
    void ApplyApiPayment(TotalProcessingPayment payment, ApiPaymentRes transaction);
    Task PreparePaymentCheckoutAsync(TotalProcessingPayment payment,
                                     PaymentsParameters parameters,
                                     PrepareCheckoutReq req);
    
    Task PrepareCredentialCheckoutAsync(TotalProcessingCredential credential,
                                        PaymentsParameters parameters,
                                        PrepareCheckoutReq req);
}
