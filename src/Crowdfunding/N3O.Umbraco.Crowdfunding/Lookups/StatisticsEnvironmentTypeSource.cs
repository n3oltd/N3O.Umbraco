using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class StatisticsEnvironmentTypeSource : LookupsDataSource<StatisticsEnvironmentType> {
    public StatisticsEnvironmentTypeSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Environment Type";
    public override string Description => "Data source for environment types";
    public override string Icon => "icon-message";

    protected override string GetIcon(StatisticsEnvironmentType statisticsEnvironmentType) => "icon-message";
}
