using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.References;

namespace N3O.Umbraco.Payments.Models {
    public class PaymentsParameters {
        public PaymentsParameters(IPaymentsFlow flow) {
            BillingInfoAccessor = flow;
            FlowId = flow.Id;
            Reference = flow.Reference;
        }

        public IBillingInfoAccessor BillingInfoAccessor { get; }
        public EntityId FlowId { get; }
        public Reference Reference { get; }
    }
}