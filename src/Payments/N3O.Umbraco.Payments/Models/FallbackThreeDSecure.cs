namespace N3O.Umbraco.Payments.Models {
    public class FallbackThreeDSecure {
        public FallbackThreeDSecure(string threeDSecureTermUrl, string threeDSecurePaReq, string threeDSecurePaRes) {
            ThreeDSecureTermUrl = threeDSecureTermUrl;
            ThreeDSecurePaReq = threeDSecurePaReq;
            ThreeDSecurePaRes = threeDSecurePaRes;
        }

        public string ThreeDSecureTermUrl { get; }
        public string ThreeDSecurePaReq { get; }
        public string ThreeDSecurePaRes { get; }
    }
}