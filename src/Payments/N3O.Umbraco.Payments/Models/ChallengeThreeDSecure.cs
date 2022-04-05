namespace N3O.Umbraco.Payments.Models {
    public class ChallengeThreeDSecure {
        public ChallengeThreeDSecure(string threeDSecureAcsTransId, string threeDSecureCReq, string threeDSecureCRes) {
            ThreeDSecureAcsTransId = threeDSecureAcsTransId;
            ThreeDSecureCReq = threeDSecureCReq;
            ThreeDSecureCRes = threeDSecureCRes;
        }

        public string ThreeDSecureAcsTransId { get; }
        public string ThreeDSecureCReq { get; }
        public string ThreeDSecureCRes { get; }
    }
}