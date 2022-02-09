using N3O.Umbraco.Payments.Lookups;
using Newtonsoft.Json;
using NodaTime;

namespace N3O.Umbraco.Payments.Models {
    public abstract partial class Credential : PaymentObject {
        protected Credential(Payment advancePayment) {
            AdvancePayment = advancePayment;
        }
        
        public Payment AdvancePayment { get; }
        public Instant? SetupAt { get; private set; }
        
        public bool IsSetUp { get; private set; }

        [JsonIgnore]
        public override PaymentObjectType Type => PaymentObjectTypes.Credential;
    }
}