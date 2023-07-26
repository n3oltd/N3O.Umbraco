using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Checkout.Models; 

public class CheckoutAccount : Account {
    [JsonConstructor]
    public CheckoutAccount(Name name,
                           Address address,
                           Accounts.Models.Email email,
                           Telephone telephone,
                           Consent consent,
                           TaxStatus taxStatus)
        : base(name, address, email, telephone, consent, taxStatus) { }

    public CheckoutAccount(Account account, bool isComplete) : base(account) {
        IsComplete = isComplete;
    }

    public bool IsComplete { get; }
}