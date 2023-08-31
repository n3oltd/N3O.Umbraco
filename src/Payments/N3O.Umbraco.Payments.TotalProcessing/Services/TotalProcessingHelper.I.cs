using N3O.Umbraco.Payments.TotalProcessing.Models;
using Payments.TotalProcessing.Clients.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing;

public interface ITotalProcessingHelper {
    public void ApplyApiPayment(TotalProcessingPayment payment, ApiTransactionRes transaction);
    public Task PrepareCheckoutAsync(TotalProcessingPayment payment, PrepareCheckoutReq req);
}
