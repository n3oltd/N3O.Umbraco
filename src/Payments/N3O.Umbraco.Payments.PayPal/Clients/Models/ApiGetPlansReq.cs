using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApiGetPlansReq {
    [JsonIgnore]
    public string ProductId { get; set; }
    
    [JsonIgnore]
    public string PageNumber { get; set; }
}