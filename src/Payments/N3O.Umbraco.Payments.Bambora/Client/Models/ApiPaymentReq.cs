using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class ApiPaymentReq {
        [JsonProperty(PropertyName = "payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty(PropertyName = "order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }

        [JsonProperty(PropertyName = "customer_ip")]
        public string CustomerIp { get; set; }

        [JsonProperty(PropertyName = "billing")]
        public ApiBillingAddressReq Billing { get; set; }

        [JsonProperty(PropertyName = "token")]
        public Token Token { get; set; }
    }
}