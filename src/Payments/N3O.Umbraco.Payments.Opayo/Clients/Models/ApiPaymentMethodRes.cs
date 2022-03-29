using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients {
    public class ApiPaymentMethodRes {
        [JsonProperty("card")]
        public ApiCardRes Card { get; set; }
    }
}