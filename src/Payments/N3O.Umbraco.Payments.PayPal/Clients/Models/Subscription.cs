using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.PayPal.Clients.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class Subscription {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("plan_id")]
    public string PlanId { get; set; }

    [JsonProperty("start_time")]
    public DateTime StartTime { get; set; }
    
    [JsonProperty("application_context")]
    public ApplicationContext ApplicationContext { get; set; }

    [JsonProperty("quantity")]
    public string Quantity { get; set; }

    [JsonProperty("shipping_amount")]
    public Amount ShippingAmount { get; set; }

    [JsonProperty("subscriber")]
    public Subscriber Subscriber { get; set; }

    [JsonProperty("billing_info")]
    public BillingInfo BillingInfo { get; set; }

    [JsonProperty("create_time")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("update_time")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("links")]
    public List<Link> Links { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("status_change_note")]
    public string StatusChangeNote { get; set; }

    [JsonProperty("status_update_time")]
    public DateTime StatusUpdatedAt { get; set; }
}