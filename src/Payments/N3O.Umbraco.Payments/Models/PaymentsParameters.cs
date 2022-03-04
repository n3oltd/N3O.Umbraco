using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Content;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.References;

namespace N3O.Umbraco.Payments.Models {
    public class PaymentsParameters {
        private readonly IPaymentsFlow _flow;

        public PaymentsParameters(IPaymentsFlow flow) {
            _flow = flow;
        }

        public IBillingInfoAccessor BillingInfoAccessor => _flow;
        public EntityId FlowId => _flow.Id;

        public string GetTransactionDescription(IPaymentMethodSettings paymentMethodSettings) => _flow.GetTransactionDescription(paymentMethodSettings);
        public string GetTransactionId(IPaymentMethodSettings paymentMethodSettings) => _flow.GetTransactionId(paymentMethodSettings);
    }
}