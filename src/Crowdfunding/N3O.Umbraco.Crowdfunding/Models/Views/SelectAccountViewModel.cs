using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crowdfunding.Models;

public class SelectAccountViewModel {
    public string Email { get; private set; }
    public IReadOnlyList<IAccount> AvailableAccounts { get; private set; }
    public DataEntrySettings DataEntrySettings { get; private set; }

    public static async Task<SelectAccountViewModel> ForAsync(IAccountManager accountManager,
                                                              IMemberManager memberManager,
                                                              IContentLocator contentLocator,
                                                              ILookups lookups) {
        var dataEntrySettingsContent = contentLocator.Single<DataEntrySettingsContent>();
        var consentOptionsContent = contentLocator.All<ConsentOptionContent>();
        var dataEntrySettings = dataEntrySettingsContent.ToDataEntrySettings(lookups, consentOptionsContent);
        
        var currentMemberEmail = await memberManager.GetCurrentPublishedMemberEmailAsync();
        var availableAccounts = await accountManager.FindAccountsByEmailAsync(currentMemberEmail);
        
        var viewModel = new SelectAccountViewModel();
        viewModel.Email = currentMemberEmail;
        viewModel.DataEntrySettings = dataEntrySettings;
        viewModel.AvailableAccounts = availableAccounts.OrEmpty().ToList();

        return viewModel;
    }
}