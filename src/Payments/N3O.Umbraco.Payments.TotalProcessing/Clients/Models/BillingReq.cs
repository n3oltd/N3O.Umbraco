using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class BillingReq {
    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("street1")]
    public string Street1 { get; set; }

    [JsonProperty("postcode")]
    public string Postcode { get; set; }
    
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("holder")]
    public string Holder { get; set; }
}
