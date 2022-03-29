using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients {
    public class ApiTransactionReq {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("credentialType")]
        public ApiCredentialTypeReq CredentialType { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("transactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("vendorTxCode")]
        public string VendorTxCode { get; set; }
    }
}