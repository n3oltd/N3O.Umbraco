using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.CrowdFunding;

public class FundraisingPageAccessControl : MembersAccessControl {
    public FundraisingPageAccessControl(IContentHelper contentHelper, IMemberManager memberManager)
        : base(contentHelper, memberManager) { }

    // TODO We need to check that the page actually allows editing, e.g. has it been unpublished, is it closed etc.
    // Need to put this logic in both AllowEditAsync methods
    protected override Task<bool> AllowEditAsync(ContentProperties contentProperties) {
        return base.AllowEditAsync(contentProperties);
    }

    protected override Task<bool> AllowEditAsync(IPublishedContent content) {
        return base.AllowEditAsync(content);
    }

    protected override string ContentTypeAlias => CrowdfundingConstants.CrowdfundingPage.Alias;
    protected override string PropertyAlias => CrowdfundingConstants.CrowdfundingPage.Properties.PageOwners;
}