using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.References;

namespace N3O.Umbraco.Payments.Entities {
    public interface IPaymentsFlow : IEntity, IBillingInfoAccessor, IHoldReference {
        PaymentObject GetPaymentObject(PaymentObjectType type);
        void SetPaymentObject(PaymentObjectType type, PaymentObject paymentObject);
    }
}