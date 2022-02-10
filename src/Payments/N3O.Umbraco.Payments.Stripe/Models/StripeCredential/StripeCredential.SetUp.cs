namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripeCredential {
        private void SetUp(string mandateId) {
            StripeMandateId = mandateId;
            
            ClearErrors();
            SetUp();
        }
    }
}