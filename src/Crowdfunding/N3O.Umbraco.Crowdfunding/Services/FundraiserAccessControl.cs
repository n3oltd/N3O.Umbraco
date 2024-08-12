using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Crowdfunding.Lookups;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Extensions;

namespace N3O.Umbraco.CrowdFunding;

public class FundraiserAccessControl : MembersAccessControl {
    public FundraiserAccessControl(IContentHelper contentHelper, IMemberManager memberManager)
        : base(contentHelper, memberManager) { }

    protected override async Task<bool> AllowEditAsync(ContentProperties contentProperties) {
        var canEdit = await base.AllowEditAsync(contentProperties);
        
        if (canEdit) {
            var status = contentProperties.GetPropertyValueByAlias<FundraiserStatus>(CrowdfundingConstants.Fundraiser.Properties.Status);
        
            canEdit = status == FundraiserStatuses.Open;
        }

        return canEdit;
    }

    protected override async Task<bool> AllowEditAsync(IPublishedContent content) {
        var canEdit = await base.AllowEditAsync(content);
        
        if (canEdit) {
            var status = content.Value<FundraiserStatus>(CrowdfundingConstants.Fundraiser.Properties.Status);
            
            canEdit = status == FundraiserStatuses.Open;
        }

        return canEdit;
    }

    protected override string ContentTypeAlias => CrowdfundingConstants.Fundraiser.Alias;
    protected override string PropertyAlias => CrowdfundingConstants.Fundraiser.Properties.Owner;
}