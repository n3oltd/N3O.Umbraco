using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public class ThreeDSecureChallenge {
    [JsonProperty("payment_method")]
    public string PaymentMethod { get; set; }

    [JsonProperty("card_response")]
    public ThreeDSecureCardResponse CardResponse { get; set; }

    [JsonIgnore]
    public string ThreeDSessionData { get; set; }
}
