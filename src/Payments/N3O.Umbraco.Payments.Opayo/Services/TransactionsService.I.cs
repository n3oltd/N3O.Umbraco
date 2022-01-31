using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo {
    public interface ITransactionsService {
        public Task ProcessAsync(OpayoPayment payment,
                                 OpayoPaymentReq req,
                                 PaymentsParameters parameters,
                                 bool saveCard);
    }
}