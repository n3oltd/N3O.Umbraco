using N3O.Umbraco.Payments.Lookups;
using System;

namespace N3O.Umbraco.Payments.Models {
    public abstract class PaymentObject : IPaymentObject {
        protected PaymentObject() {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; }
        public abstract PaymentObjectType Type { get; }
        public abstract PaymentMethod Method { get; }
        public PaymentObjectStatus Status { get; protected set; }

        public void UnhandledError(Exception ex) {
            
        }
    }
}