using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Crowdfunding.Lookups;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Extensions;

namespace N3O.Umbraco.CrowdFunding;

public class FundraisingPageAccessControl : MembersAccessControl {
    public FundraisingPageAccessControl(IContentHelper contentHelper, IMemberManager memberManager)
        : base(contentHelper, memberManager) { }

    protected override async Task<bool> AllowEditAsync(ContentProperties contentProperties) {
        if (await base.AllowEditAsync(contentProperties)) {
            var status = contentProperties.GetPropertyValueByAlias<CrowdfundingPageStatus>(CrowdfundingConstants.CrowdfundingPage.Properties.PageStatus);
            var canEdit = status == CrowdfundingPageStatuses.Open;
        
            return canEdit;
        }

        return false;
    }

    protected override async Task<bool> AllowEditAsync(IPublishedContent content) {
        if (await base.AllowEditAsync(content)) {
            var status = content.Value<CrowdfundingPageStatus>(CrowdfundingConstants.CrowdfundingPage.Properties.PageStatus);
            var canEdit = status == CrowdfundingPageStatuses.Open;
            
            return canEdit;
        }

        return false;
    }

    protected override string ContentTypeAlias => CrowdfundingConstants.CrowdfundingPage.Alias;
    protected override string PropertyAlias => CrowdfundingConstants.CrowdfundingPage.Properties.PageOwners;
}