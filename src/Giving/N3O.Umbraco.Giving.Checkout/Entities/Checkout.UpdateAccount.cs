using N3O.Umbraco.Accounts.Models;
using System;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public void UpdateAccount(Func<Account, Account> applyChanges) {
        var account = Account ?? new Account();

        Account = applyChanges(account);
    }
}