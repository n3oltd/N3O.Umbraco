using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PlatformsPageRoute : Value {
    public PlatformsPageRoute(SpecialContent parent, PublishedFileKind contentKind) {
        Parent = parent;
        ContentKind = contentKind;
    }

    public SpecialContent Parent { get; }
    public PublishedFileKind ContentKind { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Parent;
        yield return ContentKind;
    }
    
    public static readonly PlatformsPageRoute[] All = [
        // Order is important here as offerings fallback to campaign
        new(SpecialPages.Donate, PublishedFileKinds.CampaignPage),
        new(SpecialPages.Donate, PublishedFileKinds.OfferingPage),
        new(SpecialPages.Crowdfunding, PublishedFileKinds.CrowdfunderPage)
    ];
}