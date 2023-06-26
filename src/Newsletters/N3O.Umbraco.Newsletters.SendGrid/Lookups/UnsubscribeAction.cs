using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Newsletters.SendGrid.Lookups; 

public class UnsubscribeAction : NamedLookup {
    public UnsubscribeAction(string id, string name) : base(id, name) { }
}

[StaticLookups]
public class UnsubscribeActions : StaticLookupsCollection<UnsubscribeAction> {
    public static readonly UnsubscribeAction Delete = new("delete", "Delete");
    public static readonly UnsubscribeAction NoDelete = new("noDelete", "Do Not Delete");
}