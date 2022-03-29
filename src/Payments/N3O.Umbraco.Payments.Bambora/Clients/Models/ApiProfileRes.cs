using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class ApiProfileRes {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
    }
}