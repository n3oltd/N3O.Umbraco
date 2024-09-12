using N3O.Umbraco.Accounts.Models;

namespace N3O.Umbraco.CrowdFunding.Extensions;

public static class AccountNameExtensions {
    public static string ToDisplayName(this NameRes nameRes) {
        return $"{nameRes.Title} {nameRes.FirstName} {nameRes.LastName}";
    }
}