using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups;

public class NameLayoutDataSource : LookupsDataSource<NameLayout> {
    public NameLayoutDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Name Layouts";
    public override string Description => "Data source for name layouts";
    public override string Icon => "icon-layout";

    protected override string GetIcon(NameLayout nameLayout) => "icon-layout";
}
