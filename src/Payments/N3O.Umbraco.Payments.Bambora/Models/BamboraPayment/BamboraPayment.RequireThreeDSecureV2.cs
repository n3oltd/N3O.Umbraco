namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraPayment {
        public void RequireThreeDSecureV2(string returnUrl,
                                          string challengeUrl,
                                          string sessionData,
                                          string contents) {
            ClearErrors();
            
            ReturnUrl = returnUrl;
            
            base.RequireThreeDSecureV2(challengeUrl, null, sessionData, contents, null);
        }
    }
}