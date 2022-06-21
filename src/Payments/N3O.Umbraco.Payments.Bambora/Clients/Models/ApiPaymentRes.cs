using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Bambora.Client;

public class ApiPaymentRes : IApiPaymentRes {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("approved")]
    public int? Approved { get; set; }

    [JsonProperty("authorizing_merchant_id")]
    public int? AuthorizingMerchantId { get; set; }

    [JsonProperty("message_id")]
    public int? MessageId { get; set; }

    [JsonProperty("amount")]
    public decimal? Amount { get; set; }

    [JsonProperty("risk_score")]
    public decimal? RiskScore { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("auth_code")]
    public string AuthCode { get; set; }

    [JsonProperty("created")]
    public DateTime? Created { get; set; }

    [JsonProperty("type")]
    public string TransType { get; set; }

    [JsonProperty("payment_method")]
    public string PaymentMethod { get; set; }

    [JsonProperty("card")]
    public CardResponse Card { get; set; }

    [JsonProperty("3d_session_data")]
    public string ThreeDSessionData { get; set; }

    [JsonProperty("contents")]
    public string ThreeDContents { get; set; }

    [JsonProperty("links")]
    public IEnumerable<Link> Links { get; set; }
}
