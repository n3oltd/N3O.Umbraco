using Umbraco.Cms.Web.Common.Security;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class CrowdfundingSignInPageViewModel {
    public MemberExternalLoginProviderScheme Auth0LoginProvider { get; set; }
    public bool IsAuthenticated { get; set; }
}