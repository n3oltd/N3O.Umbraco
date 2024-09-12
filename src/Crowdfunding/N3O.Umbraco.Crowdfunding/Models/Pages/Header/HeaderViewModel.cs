using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Extensions;

namespace N3O.Umbraco.CrowdFunding.Models.Header;

public class HeaderViewModel {
    public MemberExternalLoginProviderScheme LoginProvider { get; set; }
    public bool IsLoggedIn { get; set; }
    public string AvatarLink { get; set; }
    
    public static async Task<HeaderViewModel> ForAsync(IMemberExternalLoginProviders memberExternalLoginProviders,
                                                       IMemberManager memberManager) {
        var viewModel = new HeaderViewModel();

        var member = await memberManager.GetCurrentPublishedMemberAsync();
        var memberLoginProviders = await memberExternalLoginProviders.GetMemberProvidersAsync();

        viewModel.LoginProvider = memberLoginProviders.Single();
        viewModel.IsLoggedIn = memberManager.IsLoggedIn();
        viewModel.AvatarLink = member?.Value<string>(MemberConstants.Member.Properties.AvatarLink);

        return viewModel;
    }
}