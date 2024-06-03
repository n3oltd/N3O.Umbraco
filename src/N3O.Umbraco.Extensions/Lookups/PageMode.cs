namespace N3O.Umbraco.Lookups;

public class PageMode : NamedLookup {
    public PageMode(string id, string name) : base(id, name) { }
}

public class PageModes : StaticLookupsCollection<PageMode> {
    public static readonly PageMode Edit = new("Edit", "edit");
    public static readonly PageMode View = new("view", "View");
}
