using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoCredentialReq {
        [Name("Advance Payment")]
        public OpayoPaymentReq AdvancePayment { get; set; }
    }
}