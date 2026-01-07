using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class OfferingDataSource : LookupsDataSource<Offering> {
    public OfferingDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Offerings";
    public override string Description => "Data source for offerings";
    public override string Icon => "icon-categories";

    protected override string GetIcon(Offering offering) => "icon-categories";
}