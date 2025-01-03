using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class CrowdfundingEnvironmentTypeSource : LookupsDataSource<CrowdfundingEnvironmentType> {
    public CrowdfundingEnvironmentTypeSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Crowdfunding Environment Type";
    public override string Description => "Data source for crowdfunding environment types";
    public override string Icon => "icon-message";

    protected override string GetIcon(CrowdfundingEnvironmentType crowdfundingEnvironmentType) => "icon-message";
}
