using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups;

public class AllocationTypeDataSource : LookupsDataSource<AllocationType> {
    public AllocationTypeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Allocation type";
    public override string Description => "Data source for allocation type";
    public override string Icon => "icon-donate";

    protected override string GetIcon(AllocationType allocationType) => "icon-donate";
}
