using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client;

public class ApiBillingAddressReq {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("address_line1")]
    public string AddressLine1 { get; set; }

    [JsonProperty("address_line2")]
    public string AddressLine2 { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("province")]
    public string Province { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("postal_code")]
    public string PostalCode { get; set; }

    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }

    [JsonProperty("email_address")]
    public string EmailAddress { get; set; }
}
