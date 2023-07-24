using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public void UpdateAccount(IAccount account, TaxReliefScheme taxReliefScheme) {
        var isEligible = taxReliefScheme?.IsEligible(account.Address.Country, false) == true;

        UpdateName(account.Name);
        UpdateAddress(account.Address);

        if (account.Email.HasValue()) {
            UpdateEmail(account.Email);
        }

        if (account.Telephone.HasValue()) {
            UpdateTelephone(account.Telephone);
        }

        if (account.Consent.HasValue()) {
            UpdateConsent(account.Consent);
        }

        if (account.TaxStatus.HasValue() && isEligible) {
            UpdateTaxStatus(account.TaxStatus);
        }

        if (Account.TaxStatus.HasValue() && !isEligible) {
            UpdateTaxStatus(null);
        }

        Account.IsTaxStatusEligible = isEligible;
    }

    public void UpdateTaxStatus(TaxStatus taxStatus) {
        Account = Account.WithUpdatedTaxStatus(taxStatus);
    }

    public void UpdateConsent(IConsent consent) {
        Account = Account.WithUpdatedConsent(new Consent(consent));
    }
    
    private void UpdateName(IName name) {
        Account = Account.WithUpdatedName(name.IfNotNull(x => new Name(x)));
    }

    private void UpdateAddress(IAddress address) {
        Account = Account.WithUpdatedAddress(address.IfNotNull(x => new Address(x)));
    }

    private void UpdateTelephone(ITelephone telephone) {
        Account = Account.WithUpdatedTelephone(telephone.IfNotNull(x => new Telephone(x)));
    }

    private void UpdateEmail(IEmail email) {
        Account = Account.WithUpdatedEmail(email.IfNotNull(x => new Accounts.Models.Email(x)));
    }
}