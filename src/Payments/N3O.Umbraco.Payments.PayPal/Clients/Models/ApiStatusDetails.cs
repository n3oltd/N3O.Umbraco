using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApiStatusDetails {
    [JsonProperty("reason")]
    public string Reason { get; set; }
}
