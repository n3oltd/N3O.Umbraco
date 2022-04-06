namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        protected void RequireThreeDSecureV1(ThreeDSecureV1 threeDSecure) {
            Card = new CardPayment(true, false, threeDSecure, null);
        }
    }
}