﻿using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.PayPal.Models;

public class PayPalSubscriptionReq {
    [Name("Subscription Id")]
    public string SubscriptionId { get; set; }
    
    [Name("Reason")]
    public string Reason { get; set; }
}