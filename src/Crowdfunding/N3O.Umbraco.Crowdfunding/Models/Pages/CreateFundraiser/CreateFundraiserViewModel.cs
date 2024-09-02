using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.CrowdFunding.Models;

public class CreateFundraiserViewModel {
    public IAccount SelectedAccount { get; set; }
    public IEnumerable<AccountRes> AvailableAccounts { get; set; }
    public bool HasSelectedAccount => SelectedAccount.HasValue();
    public bool IsLoggedIn { get; set; }
    
    // TODO Shagufta, add an account accessor and use that instead that takes the selected ID and retrieves an account
    // from it. As we are using the genercic string this can be the ID or reference. We can also add methods to the account
    // manager to find accounts based on the user email etc.
    public static async Task<CreateFundraiserViewModel> ForAsync(IAccountManager accountManager,
                                                                 IMemberManager memberManager) {
        var viewModel = new CreateFundraiserViewModel();

        var currentMemberEmail = await memberManager.GetCurrentPublishedMemberEmailAsync();

        var matchingAccounts = await accountManager.FindAccountsByEmailAsync(currentMemberEmail);

        viewModel.IsLoggedIn = memberManager.IsLoggedIn();
        
        return viewModel;
    }
}