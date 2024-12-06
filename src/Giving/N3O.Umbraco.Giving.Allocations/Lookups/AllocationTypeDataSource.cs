using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class AllocationTypeDataSource : LookupsDataSource<AllocationType> {
    public AllocationTypeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Allocation Type";
    public override string Description => "Data source for allocation type";
    public override string Icon => "icon-donate";

    protected override string GetIcon(AllocationType allocationType) => "icon-donate";
}
