using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class FeedbackCustomFieldType : NamedLookup {
    public FeedbackCustomFieldType(string id, string name) : base(id, name) { }
}

public class FeedbackCustomFieldTypes : StaticLookupsCollection<FeedbackCustomFieldType> {
    public static readonly FeedbackCustomFieldType Bool = new("bool", "Bool");
    public static readonly FeedbackCustomFieldType Date = new("date", "Date");
    public static readonly FeedbackCustomFieldType Text = new("text", "Text");
}