using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public class CardResponse {
    [JsonProperty("last_four")]
    public string LastFour { get; set; }

    [JsonProperty("cvd_match")]
    public int? CvdMatch { get; set; }

    [JsonProperty("address_match")]
    public int? AddressMatch { get; set; }

    [JsonProperty("postal_result")]
    public int? PostalMatch { get; set; }

    [JsonProperty("avs_result")]
    public string AvsResult { get; set; }

    [JsonProperty("cvd_result")]
    public string CvdResult { get; set; }

    [JsonProperty("card_type")]
    public string CardType { get; set; }
}
