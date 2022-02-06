using N3O.Umbraco.Giving.Checkout.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutProgressRes {
        public CheckoutStage CurrentStage { get; set; }
        public IEnumerable<CheckoutStage> RequiredStages { get; set; }
        public IEnumerable<CheckoutStage> RemainingStages { get; set; }
    }
}