using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class PublishedFileKind : Lookup {
    public PublishedFileKind(string id, string metaTagName) : base(id) {
        MetaTagName = metaTagName;
    }

    public string PathSegment => Id;
    public string MetaTagName { get; }
}

public class PublishedFileKinds : StaticLookupsCollection<PublishedFileKind> {
    public static readonly PublishedFileKind CampaignPage = new("campaignPage", "n3o-campaign-id");
    public static readonly PublishedFileKind CrowdfunderPage = new("crowdfunderPage", "n3o-crowdfunder-id");
    public static readonly PublishedFileKind Element = new("element", null);
    public static readonly PublishedFileKind OfferingPage = new("offeringPage", "n3o-offering-id");
    public static readonly PublishedFileKind Subscription = new("subscription", null);
}