using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsTaxStatus : Value {
    [JsonConstructor]
    public WebhookElementsTaxStatus(bool? canClaim) {
        CanClaim = canClaim;
    }

    public bool? CanClaim { get; }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return CanClaim;
    }
}