using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutStage : NamedLookup {
        private readonly Func<Entities.Checkout, bool> _isComplete;

        public CheckoutStage(string id, string name, Func<Entities.Checkout, bool> isComplete, int order)
            : base(id, name) {
            _isComplete = isComplete;
            Order = order;
        }
        
        public int Order { get; }
        
        public bool IsComplete(Entities.Checkout checkout) => _isComplete(checkout);
    }

    public class CheckoutStages : StaticLookupsCollection<CheckoutStage> {
        public static readonly CheckoutStage Account = new("account",
                                                           "Account",
                                                           c => c.Account.HasValue(),
                                                           0);

        public static readonly CheckoutStage Donation = new("donation",
                                                            "Donation",
                                                            c => c.Donation.IsComplete,
                                                            2);
        
        public static readonly CheckoutStage RegularGiving = new("regularGiving",
                                                                 "Regular Giving",
                                                                 c => c.RegularGiving.IsComplete,
                                                                 1);
    }
}