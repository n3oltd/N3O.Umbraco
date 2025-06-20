using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiPaymentMethodReq {
    [JsonProperty("card")]
    public ApiCard Card { get; set; }
    
    [JsonProperty("googlePay")]
    public ApiCardGooglePay CardGooglePay { get; set; }
}
