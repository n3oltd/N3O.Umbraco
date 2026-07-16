using N3O.Umbraco.Entities;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Payments.Content;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Models;

public class PaymentsParameters {
    private readonly IPaymentsFlow _flow;

    public PaymentsParameters(IPaymentsFlow flow) {
        _flow = flow;
    }

    public IBillingInfoAccessor BillingInfoAccessor => _flow;
    public EntityId FlowId => _flow.Id;

    public Money GetPaymentAmount(PaymentObjectType type) {
        return _flow.GetPaymentAmount(type);
    }

    public string GetTransactionDescription(IPaymentMethodSettings paymentMethodSettings) {
        return _flow.GetTransactionDescription(paymentMethodSettings);
    }

    public string GetTransactionId(IPaymentMethodSettings paymentMethodSettings, string idempotencyKey) {
        return _flow.GetTransactionId(paymentMethodSettings, idempotencyKey);
    }
}
