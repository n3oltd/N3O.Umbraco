using N3O.Umbraco.Payments.Entities;

namespace N3O.Umbraco.Payments.Models {
    public class PaymentFlowRes<T> {
        public PaymentFlowRes(IPaymentsFlow flow, T paymentObject) {
            FlowRevision = flow.Revision;
            Result = paymentObject;
        }

        public int FlowRevision { get; set; }
        public T Result { get; set; }
    }
}