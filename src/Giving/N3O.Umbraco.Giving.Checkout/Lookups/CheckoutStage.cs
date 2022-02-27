using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Content;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutStage : NamedLookup {
        private readonly Func<Entities.Checkout, bool> _isComplete;
        private readonly Func<IContentCache, IUmbracoContent> _getPage;

        public CheckoutStage(string id,
                             string name,
                             Func<Entities.Checkout, bool> isComplete,
                             int order,
                             Func<IContentCache, IUmbracoContent> getPage)
            : base(id, name) {
            _isComplete = isComplete;
            _getPage = getPage;
            Order = order;
        }
        
        public int Order { get; }
        
        public bool IsComplete(Entities.Checkout checkout) => _isComplete(checkout);
        
        public string GetUrl(IContentCache contentCache) => _getPage(contentCache).Content.AbsoluteUrl();
    }

    public class CheckoutStages : StaticLookupsCollection<CheckoutStage> {
        public static readonly CheckoutStage Account = new("account",
                                                           "Account",
                                                           c => c.Account.HasValue(),
                                                           0,
                                                           c => c.Single<CheckoutAccountPageContent>());

        public static readonly CheckoutStage Donation = new("donation",
                                                            "Donation",
                                                            c => c.Donation.IsComplete,
                                                            2,
                                                            c => c.Single<CheckoutDonationPageContent>());
        
        public static readonly CheckoutStage RegularGiving = new("regularGiving",
                                                                 "Regular Giving",
                                                                 c => c.RegularGiving.IsComplete,
                                                                 1,
                                                                 c => c.Single<CheckoutRegularGivingPageContent>());
    }
}