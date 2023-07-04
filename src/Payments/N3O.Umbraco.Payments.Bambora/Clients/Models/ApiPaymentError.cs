using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public class ApiPaymentError : IApiPaymentRes {
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("category")]
    public int Category { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("reference")]
    public string Reference { get; set; }

    [JsonProperty("transaction_id")]
    public string TransactionId { get; set; }

    [JsonProperty("order_number")]
    public string OrderNumber { get; set; }

    [JsonProperty("details")]
    public IEnumerable<Details> Details { get; set; }

    [JsonProperty("validation")]
    public CardValidation Validation { get; set; }
}
