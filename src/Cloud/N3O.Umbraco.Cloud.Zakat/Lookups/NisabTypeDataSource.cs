using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Zakat;

public class NisabTypeDataSource : LookupsDataSource<NisabType> {
    public NisabTypeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Nisab Types";
    public override string Description => "Data source for nisab types";
    public override string Icon => "icon-weight";

    protected override string GetIcon(NisabType nisabType) => "icon-weight";
}