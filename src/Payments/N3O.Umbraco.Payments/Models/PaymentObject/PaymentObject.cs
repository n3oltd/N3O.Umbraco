using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Models {
    public abstract partial class PaymentObject : IPaymentObject {
        public abstract PaymentObjectType Type { get; }
        public abstract PaymentMethod Method { get; }
        public PaymentObjectStatus Status { get; private set; }
    }
}