using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.CrowdFunding.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.CrowdFunding;

public class CreateFundraiserPage : CrowdfundingPage {
    private readonly Lazy<IAccountManager> _accountManager;
    private readonly Lazy<IAccountInfoAccessor> _accountInfoAccessor;
    private readonly Lazy<IMemberManager> _memberManager;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly ILookups _lookups;
    private readonly Lazy<IContentCache> _contentCache;

    public CreateFundraiserPage(ICrowdfundingHelper crowdfundingHelper,
                                Lazy<IAccountManager> accountManager,
                                Lazy<IAccountInfoAccessor> accountInfoAccessor,
                                Lazy<IMemberManager> memberManager,
                                Lazy<IContentLocator> contentLocator,
                                Lazy<IContentCache> contentCache,
                                ILookups lookups)
        : base(crowdfundingHelper) {
        _accountManager = accountManager;
        _accountInfoAccessor = accountInfoAccessor;
        _memberManager = memberManager;
        _contentLocator = contentLocator;
        _contentCache = contentCache;
        _lookups = lookups;
    }

    protected override bool IsMatch(string crowdfundingPath) {
        return IsMatch(crowdfundingPath, CrowdfundingUrl.Routes.CreateFundraiser);
    }

    protected override async Task<object> GetViewModelAsync(string crowdfundingPath) {
        return await CreateFundraiserViewModel.ForAsync(_accountManager.Value,
                                                        _accountInfoAccessor.Value,
                                                        _memberManager.Value,
                                                        _contentLocator.Value,
                                                        _contentCache.Value,
                                                        _lookups);
    }
}