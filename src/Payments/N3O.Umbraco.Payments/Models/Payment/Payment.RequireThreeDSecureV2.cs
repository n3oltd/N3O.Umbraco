namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        protected void RequireThreeDSecureV2(string acsUrl, string acsTransId, string sessionData, string cReq) {
            var threeDSecure = new ThreeDSecureV2(acsUrl, acsTransId, sessionData, cReq, null);
            
            Card = new CardPayment(true, false, null, threeDSecure);
        }
    }
}