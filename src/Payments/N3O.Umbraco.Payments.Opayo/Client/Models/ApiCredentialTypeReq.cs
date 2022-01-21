using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiCredentialTypeReq {
        [JsonProperty("cofUsage")]
        public string CofUsage { get; set; }

        [JsonProperty("initiatedType")]
        public string InitiatedType { get; set; }

        [JsonProperty("mitType")]
        public string MitType { get; set; }
    }
}