namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void ThreeDSecureRequired(string transactionId, string acsTransId, string threeDSecureUrl, string cReq) {
            TransactionId = transactionId;
            AcsTransId = acsTransId;
            ThreeDSecureUrl = threeDSecureUrl;
            CReq = cReq;
            RequireThreeDSecure = true;
        }
    }
}