namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void RequireThreeDSecure(string transactionId,
                                        string returnUrl,
                                        string challengeUrl,
                                        string termUrl,
                                        string acsTransId,
                                        string cReq,
                                        string paReq) {
            ClearErrors();
            
            OpayoTransactionId = transactionId;
            ReturnUrl = returnUrl;
            
            RequireThreeDSecure(challengeUrl, termUrl, acsTransId, cReq, paReq);
        }
    }
}