using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutStage : NamedLookup {
        private readonly Func<Entities.Checkout, bool> _isComplete;

        public CheckoutStage(string id, string name, int order, Func<Entities.Checkout, bool> isComplete)
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
                                                           0,
                                                           c => c.Account.HasValue());

        public static readonly CheckoutStage Donation = new("donation",
                                                            "Donation",
                                                            2,
                                                            c => c.Donation.IsComplete);
        
        public static readonly CheckoutStage RegularGiving = new("regularGiving",
                                                                 "Regular Giving",
                                                                 1,
                                                                 c => c.RegularGiving.IsComplete);
    }
}