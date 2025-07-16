using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class SponsorshipSchemeDataSource : LookupsDataSource<SponsorshipScheme> {
    public SponsorshipSchemeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Sponsorship Schemes";
    public override string Description => "Data source for sponsorship schemes";
    public override string Icon => "icon-user-female";

    protected override string GetIcon(SponsorshipScheme sponsorshipScheme) => "icon-user-female";
}
