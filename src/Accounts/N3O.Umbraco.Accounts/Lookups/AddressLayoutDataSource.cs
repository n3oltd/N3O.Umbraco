using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups;

public class AddressLayoutDataSource : LookupsDataSource<AddressLayout> {
    public AddressLayoutDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Address Layouts";
    public override string Description => "Data source for address layouts";
    public override string Icon => "icon-layout";

    protected override string GetIcon(AddressLayout addressLayout) => "icon-layout";
}
