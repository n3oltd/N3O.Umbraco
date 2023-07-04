using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public class CardValidation {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("message_id")]
    public string MessageId { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("auth_code")]
    public string AuthCode { get; set; }

    [JsonProperty("trans_date")]
    public string TransDate { get; set; }

    [JsonProperty("order_number")]
    public string OrderNumber { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("amount")]
    public string Amount { get; set; }

    [JsonProperty("cvd_id")]
    public string CvdId { get; set; }
}
