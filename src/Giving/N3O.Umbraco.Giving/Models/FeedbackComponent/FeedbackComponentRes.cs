using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackComponentRes : NamedLookupRes {
    public PricingRes Pricing { get; set; }
    public bool Mandatory { get; set; }
}
