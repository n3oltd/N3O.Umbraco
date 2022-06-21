using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiMerchantSessionKeyReq {
    [JsonProperty("vendorName")]
    public string VendorName { get; set; }
}
