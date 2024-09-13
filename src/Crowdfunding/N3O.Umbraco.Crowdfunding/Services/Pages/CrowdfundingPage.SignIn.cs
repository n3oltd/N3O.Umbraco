using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.CrowdFunding.Models;
using N3O.Umbraco.Localization;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.Common.Security;

namespace N3O.Umbraco.CrowdFunding;

public class SignInPage : CrowdfundingPage {
    private readonly IMemberExternalLoginProviders _memberExternalLoginProviders;
    private readonly IMemberManager _memberManager;

    public SignInPage(ICrowdfundingHelper crowdfundingHelper,
                      IFormatter formatter,
                      IMemberExternalLoginProviders memberExternalLoginProviders,
                      IMemberManager memberManager)
        : base(crowdfundingHelper, formatter) {
        _memberExternalLoginProviders = memberExternalLoginProviders;
        _memberManager = memberManager;
    }

    protected override bool IsMatch(string crowdfundingPath) {
        return IsMatch(crowdfundingPath, CrowdfundingUrl.Routes.SignIn);
    }

    protected override async Task<object> GetViewModelAsync(string crowdfundingPath) {
        var loginProviders = await _memberExternalLoginProviders.GetMemberProvidersAsync();

        var viewModel = SignInViewModel.For(loginProviders.Single(), _memberManager.IsLoggedIn());

        return viewModel;
    }
}