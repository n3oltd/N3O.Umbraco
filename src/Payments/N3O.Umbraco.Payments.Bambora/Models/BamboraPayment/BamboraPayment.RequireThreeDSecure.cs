namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraPayment {
        public void RequireThreeDSecure(string returnUrl, string challengeUrl, string sessionData, string contents) {
            ClearErrors();
            
            ReturnUrl = returnUrl;
            
            base.RequireThreeDSecure(challengeUrl, null, sessionData, contents, null);
        }
    }
}