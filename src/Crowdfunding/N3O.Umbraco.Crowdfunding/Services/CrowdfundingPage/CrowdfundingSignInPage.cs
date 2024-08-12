using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.CrowdFunding.Models.FundraisingPage;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.Common.Security;

namespace N3O.Umbraco.CrowdFunding.Services;

public class CrowdfundingSignInPage : CrowdfundingPageBase {
    private readonly IMemberExternalLoginProviders _memberExternalLoginProviders;
    private readonly IMemberManager _memberManager;

    public CrowdfundingSignInPage(IMemberExternalLoginProviders memberExternalLoginProviders, IMemberManager memberManager) {
        _memberExternalLoginProviders = memberExternalLoginProviders;
        _memberManager = memberManager;
    }

    public override bool IsMatch(string path) {
        return Regex.IsMatch(path, CrowdfundingPageUrlConstants.CrowdfundingSignInPagePath, RegexOptions.IgnoreCase);
    }

    public override async Task<object> GetViewModelAsync(string path) {
        var loginProviders = await _memberExternalLoginProviders.GetMemberProvidersAsync();

        var viewModel = new CrowdfundingSignInPageViewModel();
        viewModel.Auth0LoginProvider = loginProviders.Single();
        viewModel.IsAuthenticated = _memberManager.IsLoggedIn();

        return viewModel;
    }
}