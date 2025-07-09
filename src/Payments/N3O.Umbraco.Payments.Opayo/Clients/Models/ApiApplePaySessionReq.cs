using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiApplePaySessionReq {
    [JsonProperty("vendorName")]
    public string VendorName { get; set; }
    
    [JsonProperty("domain")]
    public string Domain { get; set; }
}
