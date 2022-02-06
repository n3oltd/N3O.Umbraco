using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Models {
    public abstract class Credential : PaymentObject {
        public bool IsSetUp { get; protected set; }
        public bool IsFailed { get; protected set; }
        
        public override PaymentObjectType Type => PaymentObjectTypes.Credential;
    }
}