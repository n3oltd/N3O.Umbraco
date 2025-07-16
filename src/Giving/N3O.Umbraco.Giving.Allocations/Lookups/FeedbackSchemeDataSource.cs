using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class FeedbackSchemeDataSource : LookupsDataSource<FeedbackScheme> {
    public FeedbackSchemeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Feedback Schemes";
    public override string Description => "Data source for feedback schemes";
    public override string Icon => "icon-loupe";

    protected override string GetIcon(FeedbackScheme currency) => "icon-loupe";
}
