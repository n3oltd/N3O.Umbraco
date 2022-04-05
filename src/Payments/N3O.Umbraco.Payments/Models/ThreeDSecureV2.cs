namespace N3O.Umbraco.Payments.Models {
    public class ThreeDSecureV2 : Value {
        public ThreeDSecureV2(string acsUrl, string acsTransId, string sessionData, string cReq, string cRes) {
            AcsUrl = acsUrl;
            AcsTransId = acsTransId;
            SessionData = sessionData;
            CReq = cReq;
            CRes = cRes;
        }

        public string AcsUrl { get; }
        public string AcsTransId { get; }
        public string SessionData { get; }
        public string CReq { get; }
        public string CRes { get; }

        public ThreeDSecureV2 Complete(string cRes) {
            return new ThreeDSecureV2(AcsUrl, AcsTransId, SessionData, CReq, cRes);
        }
    }
}