using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.References;

namespace N3O.Umbraco.Payments.Entities {
    public interface IPaymentsFlow : IEntity, IBillingInfoAccessor, IHoldReference {
        T GetOrCreatePaymentObject<T>() where T : PaymentObject, new();
    }
}