using N3O.Umbraco.Crowdfunding.Content;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Web.Common.Security;

namespace N3O.Umbraco.Crowdfunding.Models;

public class SignInViewModel {
    public SignInModalSettingsContent Settings { get; private set; }
    public MemberExternalLoginProviderScheme LoginProvider { get; private set; }

    public static async Task<SignInViewModel> ForAsync(IMemberExternalLoginProviders loginProviders,
                                                       SignInModalSettingsContent settingsContent) {
        var viewModel = new SignInViewModel();

        var providers = await loginProviders.GetMemberProvidersAsync();
        
        viewModel.Settings = settingsContent;
        viewModel.LoginProvider = providers.Single();

        return viewModel;
    }
}