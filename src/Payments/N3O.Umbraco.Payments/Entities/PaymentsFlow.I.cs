using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Entities {
    public interface IPaymentsFlow : IEntity, IBillingInfoAccessor, ITransactionInfoAccessor {
        T GetOrCreatePaymentObject<T>() where T : PaymentObject, new();
    }
}