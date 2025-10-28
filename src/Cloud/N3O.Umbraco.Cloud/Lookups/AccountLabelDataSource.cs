using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class AccountLabelDataSource : LookupsDataSource<AccountLabel> {
    public AccountLabelDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Account Labels";
    public override string Description => "Data source for account labels";
    public override string Icon => "icon-tag";
    
    protected override string GetIcon(AccountLabel accountLabel) => "icon-tag";
}