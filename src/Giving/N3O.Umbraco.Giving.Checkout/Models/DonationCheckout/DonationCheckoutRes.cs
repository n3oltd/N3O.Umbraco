using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class DonationCheckoutRes {
        public IEnumerable<Allocation> Allocations { get; set; }
        public MoneyRes Total { get; set; }
    }
}