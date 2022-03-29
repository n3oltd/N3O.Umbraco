using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Models.ThreeDSecureChallenge {
    public class ThreeDSecureChallengeReq {
        [JsonProperty("cres")]
        public string CRes { get; set; }

        [JsonProperty("3d_session_data")]
        public string ThreeDSessionData { get; set; }
    }
}