namespace N3O.Umbraco.Lookups;

public class DayOfMonthDataSource : LookupsDataSource<DayOfMonth> {
    public DayOfMonthDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Day of Month";
    public override string Description => "Data source for day of month";
    public override string Icon => "icon-calendar";

    protected override string GetIcon(DayOfMonth dayOfMonth) => "icon-calendar";
}
