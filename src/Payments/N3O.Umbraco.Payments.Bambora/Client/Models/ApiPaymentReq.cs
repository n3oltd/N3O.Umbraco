using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class ApiPaymentReq {
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("customer_ip")]
        public string CustomerIp { get; set; }

        [JsonProperty("billing")]
        public ApiBillingAddressReq Billing { get; set; }

        [JsonProperty("token")]
        public Token Token { get; set; }
    }
}