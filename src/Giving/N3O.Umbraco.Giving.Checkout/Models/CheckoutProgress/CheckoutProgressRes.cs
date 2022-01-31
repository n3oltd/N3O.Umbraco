using N3O.Umbraco.Giving.Checkout.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutProgressRes {
        public CheckoutStage Current { get; set; }
        public IEnumerable<CheckoutStage> Required { get; set; }
        public IEnumerable<CheckoutStage> Remaining { get; set; }
    }
}