using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class RegularGivingFrequencySource : LookupsDataSource<RegularGivingFrequency> {
    public RegularGivingFrequencySource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Regular Giving Collection Frequency";
    public override string Description => "Data source for regular giving frequency";
    public override string Icon => "icon-calendar-alt";

    protected override string GetIcon(RegularGivingFrequency regularGivingFrequency) => "icon-calendar-alt";
}
