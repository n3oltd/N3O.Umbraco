using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.TaxRelief;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Extensions;

public static class AccountExtensions {
    public static string GetName(this IAccount account, IFormatter formatter) {
        if (account.Type == AccountTypes.Individual) {
            if (account.Individual.HasValue(x => x.Name)) {
                return formatter.Text.ToDisplayName(account.Individual.Name);
            }
        } else if (account.Type == AccountTypes.Organization) {
            if (account.Organization.HasValue(x => x.Name)) {
                return account.Organization.Name;
            } else if (account.Organization.HasValue(x => x.Contact)) {
                return formatter.Text.ToDisplayName(account.Organization.Contact);
            }
        } else {
            throw UnrecognisedValueException.For(account.Type);
        }

        return account.Reference;
    }
    
    public static string GetToken(this IAccount account, IFormatter formatter) {
        var data = new {
            Id = account.Id?.Value,
            Reference = account.Reference,
            Name = GetName(account, formatter),
            Initials = GetInitials(account)
        };

        var json = JsonConvert.SerializeObject(data, Formatting.None);

        return Base64.Encode(json);
    }
    
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

    public static Account ToAccount(this IAccount account) {
        return new Account(account);
    }

    private static string GetInitials(IAccount account) {
        if (account.Type == AccountTypes.Individual) {
            if (account.Individual.HasValue(x => x.Name?.FirstName) &&
                account.Individual.HasValue(x => x.Name?.LastName)) {
                return $"{GetFirstLetter(account.Individual.Name.FirstName)}{GetFirstLetter(account.Individual.Name.LastName)}";
            } else if (account.Individual.HasValue(x => x.Name?.FirstName)) {
                return GetFirstLetter(account.Individual.Name.FirstName);
            } else if (account.Individual.HasValue(x => x.Name?.LastName)) {
                return GetFirstLetter(account.Individual.Name.LastName);
            }
        } else if (account.Type == AccountTypes.Organization) {
            if (account.Organization.HasValue(x => x.Name)) {
                return GetFirstLetters(account.Organization.Name, 2);
            }
        } else {
            throw UnrecognisedValueException.For(account.Type);
        }

        return null;
    }

    private static string GetFirstLetter(string str) {
        return GetFirstLetters(str, 1);
    }

    private static string GetFirstLetters(string str, int length) {
        return str.Left(length);
    }
}