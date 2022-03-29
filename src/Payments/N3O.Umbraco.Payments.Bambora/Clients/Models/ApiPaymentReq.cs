using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class ApiPaymentReq : ApiReq {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }
    }
}