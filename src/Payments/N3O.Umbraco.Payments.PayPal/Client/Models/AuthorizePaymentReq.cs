using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Client.Models {
    public class AuthorizePaymentReq {
        [JsonProperty("amount")]
        public AmountReq Amount { get; set; }

        [JsonProperty("invoice_id")]
        public string InvoiceId { get; set; }

        [JsonProperty("final_capture")]
        public bool FinalCapture { get; set; }

        [JsonProperty("note_to_payer")]
        public string NoteToPayer { get; set; }

        [JsonProperty("soft_descriptor")]
        public string SoftDescriptor { get; set; }

        [JsonIgnore]
        public string AuthorizationId { get; set; }
    }
}