namespace N3O.Umbraco.Lookups;

public class DayOfWeekDataSource : LookupsDataSource<DayOfWeek> {
    public DayOfWeekDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Day of Week";
    public override string Description => "Data source for day of week";
    public override string Icon => "icon-calendar";

    protected override string GetIcon(DayOfWeek dayOfWeek) => "icon-calendar";
}
