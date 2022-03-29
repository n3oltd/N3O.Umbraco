using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class Details {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}