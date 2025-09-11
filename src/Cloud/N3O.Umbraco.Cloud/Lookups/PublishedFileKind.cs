﻿using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class PublishedFileKind : Lookup {
    public PublishedFileKind(string id, string metaTagName) : base(id) {
        MetaTagName = metaTagName;
    }

    public string PathSegment => Id;
    public string MetaTagName { get; }
}

public class PublishedFileKinds : StaticLookupsCollection<PublishedFileKind> {
    public static readonly PublishedFileKind Campaign = new("campaign", "n3o-campaign-id");
    public static readonly PublishedFileKind Designation = new("designation", "n3o-designation-id");
    public static readonly PublishedFileKind Element = new("element", null);
    public static readonly PublishedFileKind Subscription = new("subscription", null);
}