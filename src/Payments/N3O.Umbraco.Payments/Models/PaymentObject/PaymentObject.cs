using N3O.Umbraco.Payments.Lookups;
using System;

namespace N3O.Umbraco.Payments.Models {
    public abstract partial class PaymentObject : IPaymentObject {
        public abstract PaymentObjectType Type { get; }
        public abstract PaymentMethod Method { get; }
        public PaymentObjectStatus Status { get; protected set; }

        public void UnhandledError(Exception ex) {
            
        }
    }
}