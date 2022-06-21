using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public class ApiAuthorizePaymentRes {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("links")]
    public IEnumerable<ApiLink> Links { get; set; }

    [JsonProperty("status_details")]
    public ApiStatusDetails StatusDetails { get; set; }
}
