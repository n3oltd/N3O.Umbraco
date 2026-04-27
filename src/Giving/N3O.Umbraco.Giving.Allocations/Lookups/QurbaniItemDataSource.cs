using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class QurbaniItemDataSource : LookupsDataSource<QurbaniItem> {
    public QurbaniItemDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Qurbani Items";
    public override string Description => "Data source for qurbani items";
    public override string Icon => "icon-calendar";

    protected override string GetIcon(QurbaniItem qurbaniItem) => "icon-calendar";
}
