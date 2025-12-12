using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class AccountLabelDataSource : LabelDataSource<AccountLabel> {
    public AccountLabelDataSource(ILookups lookups) : base(lookups) { }

    protected override TagScope Scope => TagScopes.AccountLabel;
}