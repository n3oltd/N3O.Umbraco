using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Giving.Checkout.Models {

    public class CheckoutAccountReq {
        [Name("Account")]
        public AccountReq Account { get; set; }

        [Name("Accept Terms")]
        public bool AcceptTerms { get; set; }
    }
}