using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Models {
    public class CompleteThreeDSecureReq {
        [JsonProperty("cres")]
        public string CRes { get; set; }

        [JsonProperty("3d_session_data")]
        public string ThreeDSessionData { get; set; }
    }
}