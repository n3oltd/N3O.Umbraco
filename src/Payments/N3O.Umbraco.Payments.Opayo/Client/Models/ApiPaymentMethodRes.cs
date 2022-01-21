using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiPaymentMethodRes {
        [JsonProperty("card")]
        public ApiCardRes Card { get; set; }
    }
}