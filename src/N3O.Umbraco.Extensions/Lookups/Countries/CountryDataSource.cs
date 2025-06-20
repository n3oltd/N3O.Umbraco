namespace N3O.Umbraco.Lookups;

public class CountryDataSource : LookupsDataSource<Country> {
    public CountryDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Countries";
    public override string Description => "Data source for countries";
    public override string Icon => "icon-map-location";

    protected override string GetIcon(Country country) => "icon-map-location";
}