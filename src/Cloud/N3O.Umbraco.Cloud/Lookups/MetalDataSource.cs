using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class MetalDataSource : LookupsDataSource<Metal> {
    public MetalDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Metals";
    public override string Description => "Data source for metals";
    public override string Icon => "icon-weight";

    protected override string GetIcon(Metal metal) => "icon-weight";
}