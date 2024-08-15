using Umbraco.Cms.Web.Common.Security;

namespace N3O.Umbraco.CrowdFunding.Models;

public class SignInViewModel {
    public MemberExternalLoginProviderScheme Auth0LoginProvider { get; set; }
    public bool IsAuthenticated { get; set; }

    public static SignInViewModel For(MemberExternalLoginProviderScheme auth0LoginProvider, bool isLoggedIn) {
        var viewModel = new SignInViewModel();

        viewModel.Auth0LoginProvider = auth0LoginProvider;
        viewModel.IsAuthenticated = isLoggedIn;

        return viewModel;
    }
}