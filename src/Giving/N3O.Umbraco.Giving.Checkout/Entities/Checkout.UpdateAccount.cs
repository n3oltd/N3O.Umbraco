using N3O.Umbraco.Accounts.Extensions;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Analytics;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.TaxRelief;
using System;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public void UpdateAccount(IContentCache contentCache,
                              IAttributionAccessor attributionAccessor,
                              ITaxReliefSchemeAccessor taxReliefSchemeAccessor,
                              Func<Account, Account> applyChanges) {
        var account = Account ?? new Account();

        account = applyChanges(account);

        Account = new CheckoutAccount(account, account.IsComplete(contentCache, taxReliefSchemeAccessor));
        Attribution = attributionAccessor.GetAttribution();
    }
}