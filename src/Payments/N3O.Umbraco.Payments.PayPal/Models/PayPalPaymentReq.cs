using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.PayPal.Models {
    public class PayPalPaymentReq {
        [Name("Email")]
        public string Email { get; set; }
        
        [Name("Transaction ID")]
        public string TransactionId { get; set; }
    }
}