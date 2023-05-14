using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups;

public class FeedbackCustomFieldDataSource : LookupsDataSource<FeedbackCustomField> {
    public FeedbackCustomFieldDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Feedback Custom Fields";
    public override string Description => "Custom Fields for Feedbacks";
    public override string Icon => "icon-globe";

    protected override string GetIcon(FeedbackCustomField FeedbackCustomField) => "icon-globe";
}