using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.TaxRelief.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class CheckoutAccount : Account {
    [JsonConstructor]
    public CheckoutAccount(EntityId id,
                           string reference,
                           AccountType type,
                           Individual individual,
                           Organization organization,
                           Address address,
                           Accounts.Models.Email email,
                           Telephone telephone,
                           Consent consent,
                           TaxStatus taxStatus,
                           bool isComplete)
        : base(id,
               reference,
               type, 
               individual, 
               organization, 
               address, 
               email, 
               telephone, 
               consent, 
               taxStatus) {
        IsComplete = isComplete;
    }

    public CheckoutAccount(Account account, bool isComplete) : base(account) {
        IsComplete = isComplete;
    }

    public bool IsComplete { get; }
}