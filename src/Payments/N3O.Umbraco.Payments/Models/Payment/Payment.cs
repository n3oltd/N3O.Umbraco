using N3O.Umbraco.Payments.Lookups;
using Newtonsoft.Json;
using NodaTime;

namespace N3O.Umbraco.Payments.Models {
    public abstract partial class Payment : PaymentObject {
        protected Payment() {
            if (Method.IsCardPayment) {
                Card = new CardPayment();
            }
        }
        
        public CardPayment Card { get; private set; }
        public Instant? PaidAt { get; private set; }
        public Instant? DeclinedAt { get; private set; }
        public string DeclinedReason { get; private set; }
        public bool IsDeclined { get; private set; }
        
        public bool IsPaid { get; private set; }
        
        [JsonIgnore]
        public override PaymentObjectType Type => PaymentObjectTypes.Payment;
    }
}