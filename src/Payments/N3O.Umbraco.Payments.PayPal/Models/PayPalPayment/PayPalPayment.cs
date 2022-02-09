using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.PayPal.Models {
    public partial class PayPalPayment : Payment {
        public string PayPalEmail { get; private set; }
        public string PayPalTransactionId { get; private set; }
        
        public override PaymentMethod Method => PayPalConstants.PaymentMethod;
    }
}