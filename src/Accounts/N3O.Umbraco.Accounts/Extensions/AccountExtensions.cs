using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.TaxRelief;

namespace N3O.Umbraco.Accounts.Extensions;

public static class AccountExtensions {
    public static bool IsComplete(this IAccount account,
                                  IContentCache contentCache,
                                  ITaxReliefSchemeAccessor taxReliefSchemeAccessor) {
        var emailDataEntrySettings = contentCache.Single<EmailDataEntrySettingsContent>();
        var phoneDataEntrySettings = contentCache.Single<PhoneDataEntrySettingsContent>();
        var taxReliefScheme = taxReliefSchemeAccessor.GetScheme();

        if (!account.HasValue(x => x?.Individual?.Name)) {
            return false;
        }
        
        if (!account.Type.HasValue()) {
            return false;
        }

        if (!account.Address.HasValue()) {
            return false;
        }

        if (emailDataEntrySettings.Required && !account.Email.HasValue()) {
            return false;
        }
        
        if (phoneDataEntrySettings.Required && !account.Telephone.HasValue()) {
            return false;
        }

        if (!account.Consent.HasValue()) {
            return false;
        }
        
        var isTaxReliefEligible = taxReliefScheme?.IsEligible(account.Address.Country, false) == true;

        if (isTaxReliefEligible && !account.TaxStatus.HasValue()) {
            return false;
        }

        return true;
    }

    public static IAccount ToAccount(this AccountRes accountRes) {
        return new Account(accountRes);
    }
}