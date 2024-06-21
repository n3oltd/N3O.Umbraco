using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.CrowdFunding;

public class FundraisingPageAccessControl : MembersAccessControl {
    public FundraisingPageAccessControl(IContentHelper contentHelper, IMemberManager memberManager)
        : base(contentHelper, memberManager) { }

    protected override string ContentTypeAlias => CrowdfundingConstants.CrowdfundingPage.Alias;
    protected override string PropertyAlias => CrowdfundingConstants.CrowdfundingPage.Properties.PageOwners;
}