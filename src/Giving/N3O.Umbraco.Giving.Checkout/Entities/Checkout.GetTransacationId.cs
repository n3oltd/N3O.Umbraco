using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public string GetTransactionId(IPaymentMethodSettings paymentMethodSettings, string idempotencyKey) {
        return FormatTransactionText(paymentMethodSettings.TransactionId, idempotencyKey);
    }
}
