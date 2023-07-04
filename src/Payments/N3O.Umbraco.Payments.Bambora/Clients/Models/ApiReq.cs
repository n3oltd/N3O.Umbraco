using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public abstract class ApiReq {
    [JsonProperty("token")]
    public Token Token { get; set; }

    [JsonProperty("billing")]
    public ApiBillingAddressReq BillingAddress { get; set; }

    [JsonProperty("customer_ip")]
    public string CustomerIp { get; set; }

    [JsonProperty("term_url")]
    public string ReturnUrl { get; set; }

    [JsonProperty("validate")]
    public bool Validate { get; set; } = true;
}
