namespace N3O.Umbraco.Payments.Models {
    public class FallbackThreeDSecure {
        public FallbackThreeDSecure(string termUrl, string paReq, string paRes) {
            TermUrl = termUrl;
            PaReq = paReq;
            PaRes = paRes;
        }

        public string TermUrl { get; }
        public string PaReq { get; }
        public string PaRes { get; }
    }
}