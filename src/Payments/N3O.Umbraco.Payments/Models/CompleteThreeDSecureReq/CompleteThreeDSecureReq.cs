using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Models {
    public class CompleteThreeDSecureReq {
        [Name("CRes")]
        public string CRes { get; set; }

        [Name("threeDSSessionData ")]
        public string ThreeDsSessionData { get; set; }
        
        [Name("PaRes")]
        public string PaRes { get; set; }

        [Name("MD")]
        public string MD { get; set; }
    }
}