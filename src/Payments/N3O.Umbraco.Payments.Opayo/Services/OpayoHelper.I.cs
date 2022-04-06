using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Clients;
using N3O.Umbraco.Payments.Opayo.Models;
using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo {
    public interface IOpayoHelper {
        public void ApplyAuthorisation(OpayoPayment payment, ApiTransactionRes transaction);
        public void ApplyException(OpayoPayment payment, ApiException apiException);
        public Task ChargeAsync(OpayoPayment payment, ChargeCardReq req, PaymentsParameters parameters, bool saveCard);
    }
}