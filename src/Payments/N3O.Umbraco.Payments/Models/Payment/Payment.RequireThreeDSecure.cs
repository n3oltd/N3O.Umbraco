namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        protected void RequireThreeDSecure(string challengeUrl, string acsTransId, string cReq) {
            Card = Card.RequireThreeDSecure(challengeUrl, acsTransId, cReq);
        }
    }
}