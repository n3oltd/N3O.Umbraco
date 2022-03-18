using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Client.Models {
    public class AmountReq {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
    }
}