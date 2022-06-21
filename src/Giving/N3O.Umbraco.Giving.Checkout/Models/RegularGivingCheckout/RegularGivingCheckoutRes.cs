using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Payments.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class RegularGivingCheckoutRes {
    public IEnumerable<AllocationRes> Allocations { get; set; }
    public CredentialRes Credential { get; set; }
    public RegularGivingOptionsRes Options { get; set; }
    public MoneyRes Total { get; set; }
    
    public bool IsComplete { get; set; }
    public bool IsRequired { get; set; }
}
