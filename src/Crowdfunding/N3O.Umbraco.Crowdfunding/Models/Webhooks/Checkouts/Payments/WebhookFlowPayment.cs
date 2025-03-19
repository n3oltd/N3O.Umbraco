using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookFlowPayment : Value {
    [JsonConstructor]
    public WebhookFlowPayment(bool isPaid) {
        IsPaid = isPaid;
    }
    
    public bool IsPaid { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return IsPaid;
    }
}