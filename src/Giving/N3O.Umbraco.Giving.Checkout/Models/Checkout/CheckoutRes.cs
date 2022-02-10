using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Financial;
using N3O.Umbraco.References;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutRes {
        public RevisionId RevisionId { get; set; }
        public Reference Reference { get; set; }
        public Currency Currency { get; set; }
        public CheckoutProgressRes Progress { get; set; }
        public AccountRes Account { get; set; }
        public DonationCheckoutRes Donation { get; set; }
        public RegularGivingCheckoutRes RegularGiving { get; set; }

        public bool IsComplete { get; set; }
    }
}