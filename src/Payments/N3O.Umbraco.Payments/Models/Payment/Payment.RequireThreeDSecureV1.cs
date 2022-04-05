namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        protected void RequireThreeDSecureV1(string acsUrl, string md, string paReq) {
            var threeDSecure = new ThreeDSecureV1(acsUrl, md, paReq, null);

            Card = new CardPayment(true, false, threeDSecure, null);
        }
    }
}