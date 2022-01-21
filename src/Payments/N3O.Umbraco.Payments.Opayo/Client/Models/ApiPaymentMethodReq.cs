using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiPaymentMethodReq {
        [JsonProperty("card")]
        public ApiCard Card { get; set; }
    }
}