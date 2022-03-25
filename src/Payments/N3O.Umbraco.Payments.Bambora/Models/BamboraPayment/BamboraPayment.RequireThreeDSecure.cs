namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraPayment {
        public void RequireThreeDSecure(string paymentId,
                                        string returnUrl,
                                        string challengeUrl,
                                        string threeDSessionData,
                                        string threeDContent) {
            ClearErrors();
            
            BamboraPaymentId = paymentId;
            ReturnUrl = returnUrl;
            
            RequireThreeDSecure(challengeUrl, threeDSessionData, threeDContent);
        }
    }
}