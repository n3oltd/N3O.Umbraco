﻿using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCredential;

public partial class PayPalCredential : Credential{
    public override PaymentMethod Method => PayPalConstants.PaymentMethod;
    
    public string PayPalSubscriptionId { get; private set; }
}