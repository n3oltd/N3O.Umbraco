namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        protected void RequireThreeDSecure(string challengeUrl, string acsTransId, string cReq) {
            Card = new CardPayment(true, false, challengeUrl, acsTransId, cReq, null);
        }
    }
}