﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Models;

public class FlowPaymentMethod {
    public Guid Id { get; set; }
    public string ProcessorId { get; set; }
    public string Name { get; set; }
    public IEnumerable<FlowPaymentMethodCurrency> AllowedCurrencies { get; set; }
    public bool SupportsApplePay { get; set; }
    public bool SupportsGooglePay { get; set; }
    public bool SupportsRealtimePayments { get; set; }
    
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; }
}