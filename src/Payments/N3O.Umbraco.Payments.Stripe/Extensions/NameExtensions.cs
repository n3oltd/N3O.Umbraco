using N3O.Umbraco.Accounts.Models;

namespace N3O.Umbraco.Payments.Stripe;

public static class NameExtensions {
    public static string ToFullName(this IName name) {
        return $"{name.Title} {name.FirstName} {name.LastName}";
    }
}
