using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Financial;

public class CurrencyDataSource : LookupsDataSource<Currency> {
    public CurrencyDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Currencies";
    public override string Description => "Data source for currencies";
    public override string Icon => "icon-coin-dollar";

    protected override string GetIcon(Currency currency) => "icon-coin-dollar";
}
