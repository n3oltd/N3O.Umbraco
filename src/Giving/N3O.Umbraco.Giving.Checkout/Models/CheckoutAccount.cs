using N3O.Umbraco.Accounts.Models;

namespace N3O.Umbraco.Giving.Checkout.Models; 

public class CheckoutAccount : Account {
    public CheckoutAccount(Account account, bool isComplete) : base(account) {
        IsComplete = isComplete;
    }

    public bool IsComplete { get; }
}