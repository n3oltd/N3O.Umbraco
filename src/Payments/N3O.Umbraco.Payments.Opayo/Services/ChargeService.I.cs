using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo {
    public interface IChargeService {
        public Task ChargeAsync(OpayoPayment payment,
                                 ChargeCardReq req,
                                 PaymentsParameters parameters,
                                 bool saveCard);
    }
}