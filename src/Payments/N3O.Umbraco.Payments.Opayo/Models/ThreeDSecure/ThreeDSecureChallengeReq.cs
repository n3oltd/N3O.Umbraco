using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class ThreeDSecureChallengeReq {
        [Name("cres")]
        public string CRes { get; set; }

        [Name("threeDSSessionData")]
        public string ThreeDsSessionData { get; set; }
    }
}