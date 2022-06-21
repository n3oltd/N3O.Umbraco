namespace N3O.Umbraco.Payments.Models;

public partial class Payment {
    protected void RequireThreeDSecureV2(ThreeDSecureV2 threeDSecure) {
        Card = new CardPayment(true, false, null, threeDSecure);
    }
}
