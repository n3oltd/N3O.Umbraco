using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.PayPal.Models {
    public partial class PayPalPayment : Payment {
        public string TransactionId { get; private set; }


        public override PaymentMethod Method => PayPalConstants.PaymentMethod;
    }
}