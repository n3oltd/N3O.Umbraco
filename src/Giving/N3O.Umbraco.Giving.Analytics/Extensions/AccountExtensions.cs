using System.Text.RegularExpressions;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Giving.Analytics.Extensions;

public static class AccountExtensions {
    public static object ToMetaAdvancedMatchingUser(this Account account) {
        if (account == null) {
            return new object();
        }
        
        return new {
            email = account.Email?.Address?.Sha256(),
            phone = account.Telephone.IfNotNull(x => x.Number, x => Regex.Replace(x, @"\D", "")),
            firstName = account.Individual?.Name?.FirstName?.ToLowerInvariant(),
            lastName = account.Individual?.Name?.LastName?.ToLowerInvariant(),
            city = account.Address?.Locality?.RemoveWhitespace()?.ToLowerInvariant(),
            zip = account.Address?.PostalCode,
            country = account.Address?.Country?.Iso2Code
        };
    }
}