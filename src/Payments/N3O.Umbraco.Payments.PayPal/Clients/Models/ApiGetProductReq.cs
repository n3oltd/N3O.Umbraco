using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApiGetProductReq {
    [JsonIgnore]
    public string Id { get; set; }
}