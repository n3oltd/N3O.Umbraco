using N3O.Umbraco.Accounts.Models;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public void UpdateAccount(IAccount account) {
            Account = new Account(account);
        }
    }
}