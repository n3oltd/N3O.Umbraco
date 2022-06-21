using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiPaymentTransactionReq : ApiTransactionReq {
    [JsonProperty("apply3DSecure")]
    public string ApplyThreeDSecure { get; set; }

    [JsonProperty("billingAddress")]
    public ApiBillingAddressReq BillingAddress { get; set; }

    [JsonProperty("customerFirstName")]
    public string CustomerFirstName { get; set; }

    [JsonProperty("customerLastName")]
    public string CustomerLastName { get; set; }

    [JsonProperty("entryMethod")]
    public string EntryMethod { get; set; }

    [JsonProperty("paymentMethod")]
    public ApiPaymentMethodReq PaymentMethod { get; set; }

    [JsonProperty("strongCustomerAuthentication")]
    public ApiStrongCustomerAuthentication StrongCustomerAuthentication { get; set; }
}
