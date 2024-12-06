using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class FeedbackCustomFieldTypeDataSource : LookupsDataSource<FeedbackCustomFieldType> {
    public FeedbackCustomFieldTypeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Feedback Custom Fields";
    public override string Description => "Data source for feedback custom fields";
    public override string Icon => " icon-edit";

    protected override string GetIcon(FeedbackCustomFieldType feedbackCustomField) => " icon-edit";
}