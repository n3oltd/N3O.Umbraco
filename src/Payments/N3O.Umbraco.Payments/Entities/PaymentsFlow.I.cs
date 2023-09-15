using N3O.Umbraco.Entities;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Payments.Content;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.References;
using NodaTime;

namespace N3O.Umbraco.Payments.Entities;

public interface IPaymentsFlow : IEntity, IBillingInfoAccessor, IHoldReference {
    void BeginPaymentFlow(IClock clock);
    void EndPaymentFlow();
    PaymentObject GetPaymentObject(PaymentObjectType type);
    public string GetTransactionDescription(IPaymentMethodSettings paymentMethodSettings);
    string GetTransactionId(IPaymentMethodSettings paymentMethodSettings, string idempotencyKey);
    Money GetValue();
    void SetPaymentObject(PaymentObjectType type, PaymentObject paymentObject);
}
