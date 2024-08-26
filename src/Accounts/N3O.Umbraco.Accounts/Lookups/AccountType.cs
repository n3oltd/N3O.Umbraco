using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups;

public class AccountType : NamedLookup {
    public AccountType(string id, string name) : base(id, name) { }
}

public class AccountTypes : StaticLookupsCollection<AccountType> {
    public static readonly AccountType Individual = new("individual", "Individual");
    public static readonly AccountType Organization = new("organization", "Organization");
}