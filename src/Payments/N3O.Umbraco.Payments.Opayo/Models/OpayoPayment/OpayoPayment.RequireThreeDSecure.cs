namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void RequireThreeDSecure(string transactionId,
                                        string returnUrl,
                                        string challengeUrl,
                                        string acsTransId,
                                        string cReq) {
            OpayoTransactionId = transactionId;
            ReturnUrl = returnUrl;

            RequireThreeDSecure(challengeUrl, acsTransId, cReq);
        }
    }
}