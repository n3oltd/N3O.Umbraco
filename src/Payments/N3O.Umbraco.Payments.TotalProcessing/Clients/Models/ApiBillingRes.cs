using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class ApiBillingRes {
    [JsonProperty("street1")]
    public string Street1 { get; set; }

    [JsonProperty("street2")]
    public string Street2 { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("postcode")]
    public string Postcode { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }
}
