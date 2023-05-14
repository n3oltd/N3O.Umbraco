using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups;

public class FeedbackCustomField : NamedLookup {
    public FeedbackCustomField(string id, string name) : base(id, name) { }
    
    public string Slug => Id;
}

public class FeedbackCustomFields : StaticLookupsCollection<FeedbackCustomField> {
    public static readonly FeedbackCustomField Text = new("text", "Text");
    public static readonly FeedbackCustomField Date = new("date", "Date");
}