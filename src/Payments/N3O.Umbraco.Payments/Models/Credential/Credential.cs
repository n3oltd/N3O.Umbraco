using N3O.Umbraco.Payments.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Models {
    public abstract class Credential : PaymentObject {
        public virtual Payment AdvancePayment { get; private set; }
        
        public bool IsSetUp { get; private set; }

        [JsonIgnore]
        public override PaymentObjectType Type => PaymentObjectTypes.Credential;
    }
}