using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Models;

public class FlowPaymentMethod {
    public Guid Id { get; set; }
    public string ProcessorId { get; set; }
    public string Name { get; set; }
    public IEnumerable<FlowPaymentMethodCurrency> AllowedCurrencies { get; set; }
}