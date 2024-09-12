using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.TaxRelief.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.CrowdFunding.Models;

public class CreateFundraiserViewModel {
    public string SelectedAccountId { get; set; }
    public IEnumerable<AccountRes> AvailableAccounts { get; set; }
    public bool HasSelectedAccount => SelectedAccountId.HasValue();
    public bool IsLoggedIn { get; set; }
    public GuestFundraiserViewModel GuestFundraiser { get; set; }
    public DataEntrySettings DataEntrySettings { get; set; }
    public TaxReliefScheme TaxReliefScheme { get; set; }

    public static async Task<CreateFundraiserViewModel> ForAsync(IAccountManager accountManager,
                                                                 IAccountInfoAccessor accountInfoAccessor,
                                                                 IMemberManager memberManager,
                                                                 IContentLocator contentLocator,
                                                                 IContentCache contentCache,
                                                                 ILookups lookups) {
        var viewModel = new CreateFundraiserViewModel();
        var guestFundraiserContent = contentLocator.Single<GuestFundraiserContent>();

        viewModel.IsLoggedIn = memberManager.IsLoggedIn();

        if (!viewModel.IsLoggedIn) {
            viewModel.GuestFundraiser = GuestFundraiserViewModel.For(guestFundraiserContent);
        } else {
            var dataEntrySettingsContent = contentCache.Single<DataEntrySettingsContent>();
            var consentOptionsContent = contentCache.All<ConsentOptionContent>();
            var dataEntrySettings = dataEntrySettingsContent.ToDataEntrySettings(lookups, consentOptionsContent);

            viewModel.DataEntrySettings = dataEntrySettings;

            var currentMemberEmail = await memberManager.GetCurrentPublishedMemberEmailAsync();

            var matchingAccounts = await accountManager.FindAccountsByEmailAsync(currentMemberEmail);

            viewModel.AvailableAccounts = matchingAccounts;

            if (accountInfoAccessor.GetId().HasValue()) {
                viewModel.SelectedAccountId = accountInfoAccessor.GetId();
            }
        }

        return viewModel;
    }
}