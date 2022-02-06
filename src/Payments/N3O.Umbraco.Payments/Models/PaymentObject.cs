using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Lookups;
using System;

namespace N3O.Umbraco.Payments.Models {
    public abstract class PaymentObject : Value {
        protected PaymentObject() {
            Id = EntityId.New();
        }
        
        public EntityId Id { get; private set; }
        public abstract PaymentObjectType Type { get; }
        public abstract PaymentMethod Method { get; }
        public PaymentObjectStatus Status { get; protected set; }

        public void UnhandledError(Exception ex) {
            // TODO
        }
    }
}