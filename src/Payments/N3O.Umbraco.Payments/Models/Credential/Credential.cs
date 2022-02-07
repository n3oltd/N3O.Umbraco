using N3O.Umbraco.Payments.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Models {
    public abstract class Credential : PaymentObject {
        protected Credential(Payment advancePayment) {
            AdvancePayment = advancePayment;
        }
        
        public Payment AdvancePayment { get; }
        
        public bool IsSetUp { get; private set; }

        [JsonIgnore]
        public override PaymentObjectType Type => PaymentObjectTypes.Credential;
    }
}