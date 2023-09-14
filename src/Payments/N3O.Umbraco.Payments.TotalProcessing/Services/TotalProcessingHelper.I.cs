using N3O.Umbraco.Payments.TotalProcessing.Models;
using Payments.TotalProcessing.Clients.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing;

public interface ITotalProcessingHelper {
    void ApplyApiTransaction(TotalProcessingPayment payment, ApiTransactionRes transaction);
    Task PreparePaymentCheckoutAsync(TotalProcessingPayment payment,
                                     PaymentsParameters parameters,
                                     PrepareCheckoutReq req);
    
    Task PrepareCredentialCheckoutAsync(TotalProcessingCredential credential,
                                        PaymentsParameters parameters,
                                        PrepareCheckoutReq req);
}
