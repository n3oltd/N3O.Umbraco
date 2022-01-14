using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutLookupsRes : LookupsRes {
        [FromLookupType(CheckoutLookupTypes.Countries)]
        public IEnumerable<CountryRes> Countries { get; set; }
    }
}