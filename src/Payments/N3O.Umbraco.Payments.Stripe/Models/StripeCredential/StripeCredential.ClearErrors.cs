namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripeCredential {
        private void ClearErrors() {
            StripeErrorCode = null;
            StripeDeclineCode = null;
            StripeErrorMessage = null;
        }
    }
}