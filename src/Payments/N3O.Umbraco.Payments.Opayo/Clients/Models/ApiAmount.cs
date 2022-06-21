using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiAmount {
    [JsonProperty("totalAmount")]
    public long TotalAmount { get; set; }
}
