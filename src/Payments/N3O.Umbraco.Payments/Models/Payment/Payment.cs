using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Models {
    public abstract class Payment : PaymentObject {
        public override PaymentObjectType Type => PaymentObjectTypes.Payment;
    }
}