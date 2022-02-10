namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripePayment {
        private void ClearErrors() {
            StripeErrorCode = null;
            StripeDeclineCode = null;
            StripeErrorMessage = null;
        }
    }
}