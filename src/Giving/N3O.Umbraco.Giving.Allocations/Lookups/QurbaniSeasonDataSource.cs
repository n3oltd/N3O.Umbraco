using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class QurbaniSeasonDataSource : LookupsDataSource<QurbaniSeason> {
    public QurbaniSeasonDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Qurbani Seasons";
    public override string Description => "Data source for qurbani seasons";
    public override string Icon => "icon-calendar";

    protected override string GetIcon(QurbaniSeason qurbaniSeason) => "icon-calendar";
}
