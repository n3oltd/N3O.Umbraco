using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsTaxStatus : Value {
    [JsonConstructor]
    public WebhookElementsTaxStatus(bool? canClaim) {
        CanClaim = canClaim;
    }

    public bool? CanClaim { get; }
    
    public TaxStatus ToTaxStatus() {
        if (CanClaim == true) {
            return TaxStatuses.Payer;
        } else if (CanClaim == false) {
            return TaxStatuses.NonPayer;
        } else {
            return TaxStatuses.NotSpecified;
        }
    }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return CanClaim;
    }
}