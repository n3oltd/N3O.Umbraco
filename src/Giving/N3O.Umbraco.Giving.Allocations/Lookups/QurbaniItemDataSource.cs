using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class QurbaniItemDataSource : LookupsDataSource<QurbaniItem> {
    public QurbaniItemDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Qurbani Items";
    public override string Description => "Data source for qurbani items";
    public override string Icon => "icon-shopping-basket-alt-2";

    protected override string GetIcon(QurbaniItem qurbaniItem) => "icon-shopping-basket-alt-2";
}
