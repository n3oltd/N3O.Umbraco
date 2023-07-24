using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public void UpdateAccount(IAccount account) {
        Account = new Account(account);
    }
    
    public void UpdateAccountTaxStatus(TaxStatus taxStatus) {
        if (Account.HasValue()) {
            Account = Account.UpdateAccountTaxStatus(taxStatus);
        } else {
            Account = new Account(null, null, null, null, null, taxStatus);
        }
    }
    
    public void UpdateAccountConsent(ConsentReq consent) {
        if (Account.HasValue()) {
            Account = Account.UpdateAccountConsent(consent.IfNotNull(x => new Consent(x)));
        } else {
            Account = new Account(null, null, null, null, new Consent(consent), null);
        }
    }
    
    public void UpdateAccountInformation(AccountInformationReq req) {
        if (Account.HasValue()) {
            Account = Account.UpdateAccountInformation(req.Name, req.Address, req.Email, req.Telephone);
        } else {
            Account = new Account(req.Name, req.Address, req.Email, req.Telephone, null, null);
        }
    }
}
