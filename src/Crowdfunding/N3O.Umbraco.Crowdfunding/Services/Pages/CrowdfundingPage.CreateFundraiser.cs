using N3O.Umbraco.Crm;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.CrowdFunding.Models;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.CrowdFunding;

public class CreateFundraiserPage : CrowdfundingPage {
    private readonly Lazy<IAccountManager> _accountManager;
    private readonly Lazy<IMemberManager> _memberManager;

    public CreateFundraiserPage(ICrowdfundingHelper crowdfundingHelper,
                                Lazy<IAccountManager> accountManager,
                                Lazy<IMemberManager> memberManager)
        : base(crowdfundingHelper) {
        _accountManager = accountManager;
        _memberManager = memberManager;
    }

    protected override bool IsMatch(string crowdfundingPath) {
        return IsMatch(crowdfundingPath, CrowdfundingUrl.Routes.CreateFundraiser);
    }

    protected override async Task<object> GetViewModelAsync(string crowdfundingPath) {
        return await CreateFundraiserViewModel.ForAsync(_accountManager.Value, _memberManager.Value);
    }
}