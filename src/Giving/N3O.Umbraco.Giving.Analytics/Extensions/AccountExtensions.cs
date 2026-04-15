using System.Text.RegularExpressions;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Giving.Analytics.Extensions;

public static class AccountExtensions {
    public static object ToMetaAdvancedMatchingUser(this Account account) {
        return new {
                       email = account.Email.Address.Sha256(),
                       phone = FormatPhoneNumber(account.Telephone.Number),
                       firstName = account.Individual.Name.FirstName.ToLowerInvariant(),
                       lastName = account.Individual.Name.LastName.ToLowerInvariant(),
                       city = account.Address.Locality.RemoveWhitespace().ToLowerInvariant(),
                       state = account.Address.AdministrativeArea.ToLowerInvariant()[..2],
                       zip = account.Address.PostalCode,
                       country = account.Address.Country.Iso2Code,
                   };
    }

    private static string FormatPhoneNumber(string phoneNumber) {
        return string.IsNullOrWhiteSpace(phoneNumber)
            ? phoneNumber
            : Regex.Replace(phoneNumber, @"\D", string.Empty);
    }
}