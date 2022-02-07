using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoCredential : Credential {
        public OpayoCredential() : base(new OpayoPayment()) { }
        
        public override PaymentMethod Method => OpayoConstants.PaymentMethod;

        [JsonIgnore]
        public new OpayoPayment AdvancePayment => (OpayoPayment) base.AdvancePayment;
    }
}