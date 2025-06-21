using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class PublishedFileKind : Lookup {
    public PublishedFileKind(string id) : base(id) { }

    public string PathSegment => Id;
}

public class PublishedFileKinds : StaticLookupsCollection<PublishedFileKind> {
    public static readonly PublishedFileKind Campaign = new("campaign");
    public static readonly PublishedFileKind Designation = new("designation");
    public static readonly PublishedFileKind Element = new("element");
    public static readonly PublishedFileKind Subscription = new("subscription");
}