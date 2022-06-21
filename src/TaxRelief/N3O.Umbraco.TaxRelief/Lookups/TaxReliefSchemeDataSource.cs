using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.TaxRelief.Lookups;

public class TaxReliefSchemeDataSource : LookupsDataSource<TaxReliefScheme> {
    public TaxReliefSchemeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Tax Relief Schemes";
    public override string Description => "Data source for tax relief schemes";
    public override string Icon => "icon-diploma";

    protected override string GetDescription(TaxReliefScheme scheme) => null;
    protected override string GetIcon(TaxReliefScheme scheme) => Icon;
}
