using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Sync;

namespace N3O.Umbraco.Crowdfunding.Sync;

public class CrowdfundingContentSyncFilter : IContentSyncFilter {
    private static string ContentTypeAlias = AliasHelper<CrowdfunderContent<CampaignContent>>.ContentTypeAlias();
    
    private static string[] ExcludedProperties = [
        AliasHelper<CrowdfunderContent<CampaignContent>>.PropertyAlias(x => x.Status),
        AliasHelper<CrowdfunderContent<CampaignContent>>.PropertyAlias(x => x.ToggleStatus)
    ];
    
    public bool IsFilter(string contentTypeAlias) {
        return contentTypeAlias.Equals(ContentTypeAlias);
    }
    
    public bool ShouldImport(string propertyAlias) {
        return ExcludedProperties.DoesNotContain(propertyAlias, true);
    }
}