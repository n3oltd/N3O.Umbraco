using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class CheckoutLookupsRes : LookupsRes {
    [FromLookupType(CheckoutLookupTypes.CheckoutStages)]
    public IEnumerable<NamedLookupRes> CheckoutStages { get; set; }

    [FromLookupType(CheckoutLookupTypes.RegularGivingFrequencies)]
    public IEnumerable<NamedLookupRes> RegularGivingFrequencies { get; set; }
}
