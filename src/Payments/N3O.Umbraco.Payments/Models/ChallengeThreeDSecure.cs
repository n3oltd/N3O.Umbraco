namespace N3O.Umbraco.Payments.Models {
    public class ChallengeThreeDSecure {
        public ChallengeThreeDSecure(string acsTransId, string cReq, string cRes) {
            AcsTransId = acsTransId;
            CReq = cReq;
            CRes = cRes;
        }

        public string AcsTransId { get; }
        public string CReq { get; }
        public string CRes { get; }
    }
}