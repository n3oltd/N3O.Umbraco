using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApiGetPlanReq {
    [JsonIgnore]
    public string Id { get; set; }
}