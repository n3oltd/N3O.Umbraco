using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class GivingScheduleDataSource : LookupsDataSource<GivingSchedule> {
    public GivingScheduleDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Giving Schedules";
    public override string Description => "Data source for giving schedules";
    public override string Icon => "icon-calendar";

    protected override string GetIcon(GivingSchedule givingSchedule) => "icon-calendar";
}
