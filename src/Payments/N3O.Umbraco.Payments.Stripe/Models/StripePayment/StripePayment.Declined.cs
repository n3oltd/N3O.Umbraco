namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripePayment {
        public void Declined(string errorCode, string declinedCode, string errorMessage) {
            StripeErrorCode = errorCode;
            StripeDeclineCode = declinedCode;
            StripeErrorMessage = errorMessage;
            
            Declined(errorMessage);
        }
    }
}