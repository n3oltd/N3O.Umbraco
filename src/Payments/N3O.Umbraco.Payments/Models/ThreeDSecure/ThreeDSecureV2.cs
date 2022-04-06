namespace N3O.Umbraco.Payments.Models {
    public class ThreeDSecureV2 : Value {
        public ThreeDSecureV2(string acsUrl,
                              string acsTransId,
                              string sessionData,
                              string cReq,
                              string cRes,
                              string html) {
            AcsUrl = acsUrl;
            AcsTransId = acsTransId;
            SessionData = sessionData;
            CReq = cReq;
            CRes = cRes;
            Html = html;
        }

        public string AcsUrl { get; }
        public string AcsTransId { get; }
        public string SessionData { get; }
        public string CReq { get; }
        public string CRes { get; }
        public string Html { get; }

        public ThreeDSecureV2 Complete(string cRes) {
            return new ThreeDSecureV2(AcsUrl, AcsTransId, SessionData, CReq, cRes, Html);
        }

        public static ThreeDSecureV2 FromHtml(string html, string sessionData) {
            return new ThreeDSecureV2(null, null, sessionData, null, null, html);
        }
        
        public static ThreeDSecureV2 FromParameters(string acsUrl, string acsTransId, string sessionData, string cReq) {
            return new ThreeDSecureV2(acsUrl, acsTransId, sessionData, cReq, null, null);
        }
    }
}