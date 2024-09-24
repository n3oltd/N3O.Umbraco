using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crowdfunding.Models;

public class AccountsViewModel {
    public string Email { get; private set; }
    public IReadOnlyList<IAccount> Available { get; private set; }
    public IAccount Picked { get; private set; }
    public IAccount Selected { get; private set; }
    public DataEntrySettings DataEntrySettings { get; private set; }

    public static async Task<AccountsViewModel> ForAsync(IAccountManager accountManager,
                                                         IMemberManager memberManager,
                                                         IContentLocator contentLocator,
                                                         ILookups lookups,
                                                         AccountIdentity account,
                                                         IReadOnlyDictionary<string, string> query) {
        var dataEntrySettingsContent = contentLocator.Single<DataEntrySettingsContent>();
        var consentOptionsContent = contentLocator.All<ConsentOptionContent>();
        var dataEntrySettings = dataEntrySettingsContent.ToDataEntrySettings(lookups, consentOptionsContent);
        var pickedAccountId = query.GetValueOrDefault("account");
        
        var currentMemberEmail = await memberManager.GetCurrentPublishedMemberEmailAsync();
        var availableAccounts = currentMemberEmail.HasValue()
                                    ? await accountManager.FindAccountsByEmailAsync(currentMemberEmail)
                                    : Enumerable.Empty<AccountRes>();
        
        var viewModel = new AccountsViewModel();
        viewModel.Email = currentMemberEmail;
        viewModel.DataEntrySettings = dataEntrySettings;
        viewModel.Available = availableAccounts.OrEmpty().ToList();
        viewModel.Picked = viewModel.Available.SingleOrDefault(x => x.Id == pickedAccountId);
        viewModel.Selected = viewModel.Available.SingleOrDefault(x => x.Id == account.Id);

        return viewModel;
    }
}