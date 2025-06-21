using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class FundDimensionSelectorDataSource : LookupsDataSource<FundDimensionSelector> {
    public FundDimensionSelectorDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Fund Dimension Selectors";
    public override string Description => "Data source for fund dimension selectors";
    public override string Icon => "icon-mouse-cursor";
    
    protected override string GetIcon(FundDimensionSelector fundDimensionSelector) => "icon-mouse-cursor";
}