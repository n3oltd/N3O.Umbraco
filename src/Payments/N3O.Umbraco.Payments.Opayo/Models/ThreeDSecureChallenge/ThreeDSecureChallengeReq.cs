using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class ThreeDSecureChallengeReq {
        [Name("CRes")]
        public string CRes { get; set; }

        [Name("ThreeDsSessionData")]
        public string ThreeDsSessionData { get; set; }
    }
}