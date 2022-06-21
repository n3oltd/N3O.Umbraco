using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiTransactionRes {
    [JsonProperty("amount")]
    public ApiAmount Amount { get; set; }
    
    [JsonProperty("avsCvcCheck")]
    public ApiAvsCvsCheck AvsCvsCheck { get; set; }
    
    [JsonProperty("bankAuthorisationCode")]
    public string BankAuthorisationCode { get; set; }
    
    [JsonProperty("bankResponseCode")]
    public string BankResponseCode { get; set; }
    
    [JsonProperty("currency")]
    public string Currency { get; set; }
    
    [JsonProperty("paymentMethod")]
    public ApiPaymentMethodRes PaymentMethod { get; set; }
    
    [JsonProperty("retrievalReference")]
    public long? RetrievalReference { get; set; }
    
    [JsonProperty("transactionType")]
    public string TransactionType { get; set; }

    [JsonProperty("3DSecure")]
    public ApiThreeDSecure ThreeDSecure { get; set; }

    [JsonProperty("statusCode")]
    public int StatusCode { get; set; }

    [JsonProperty("statusDetail")]
    public string StatusDetail { get; set; }

    [JsonProperty("transactionId")]
    public string TransactionId { get; set; }

    [JsonProperty("acsTransId")]
    public string AcsTransId { get; set; }

    [JsonProperty("dsTranId")]
    public string DsTranId { get; set; }

    [JsonProperty("acsUrl")]
    public string AcsUrl { get; set; }

    [JsonProperty("cReq")]
    public string CReq { get; set; }
    
    [JsonProperty("paReq")]
    public string PaReq { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
}
