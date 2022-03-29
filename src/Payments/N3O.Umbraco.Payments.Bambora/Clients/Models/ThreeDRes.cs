using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class ThreeDRes {
        [JsonProperty("3d_session_data")]
        public string ThreeDSessionData { get; set; }

        [JsonProperty("contents")]
        public string Contents { get; set; }

        [JsonProperty("links")]
        public IEnumerable<Link> Links { get; set; }
    }
}