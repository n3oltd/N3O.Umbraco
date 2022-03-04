using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public string GetTransactionDescription(IPaymentMethodSettings paymentMethodSettings) {
            return FormatTransactionText(paymentMethodSettings.TransactionDescription);
        }
    }
}