using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoCredential : Credential {
        public OpayoCredential() {
            AdvancePayment = new OpayoPayment();
        }
        
        public override PaymentMethod Method => OpayoConstants.PaymentMethod;
        
        public OpayoPayment AdvancePayment { get; private set; }
    }
}