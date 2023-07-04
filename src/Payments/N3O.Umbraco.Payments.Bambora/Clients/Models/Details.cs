using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public class Details {
    [JsonProperty("field")]
    public string Field { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}
