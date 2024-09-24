using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.Common.Security;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingViewModelFactory : ICrowdfundingViewModelFactory {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<IMemberManager> _memberManager;
    private readonly Lazy<IMemberExternalLoginProviders> _memberExternalLoginProviders;
    private readonly Lazy<IAccountIdentityAccessor> _accountIdentityAccessor;
    private readonly Lazy<IAccountManager> _accountManager;
    private readonly Lazy<ILookups> _lookups;
    private readonly Lazy<IFormatter> _formatter;

    public CrowdfundingViewModelFactory(Lazy<IContentLocator> contentLocator,
                                        Lazy<IMemberManager> memberManager,
                                        Lazy<IMemberExternalLoginProviders> memberExternalLoginProviders,
                                        Lazy<IAccountIdentityAccessor> accountIdentityAccessor,
                                        Lazy<IAccountManager> accountManager,
                                        Lazy<ILookups> lookups,
                                        Lazy<IFormatter> formatter) {
        _contentLocator = contentLocator;
        _memberManager = memberManager;
        _memberExternalLoginProviders = memberExternalLoginProviders;
        _accountIdentityAccessor = accountIdentityAccessor;
        _accountManager = accountManager;
        _lookups = lookups;
        _formatter = formatter;
    }
    
    public async Task<T> CreateViewModelAsync<T>(ICrowdfundingPage page, IReadOnlyDictionary<string, string> query)
        where T : CrowdfundingViewModel, new() {
        var viewModel = CrowdfundingViewModel.For<T>(_contentLocator.Value,
                                                     _formatter.Value,
                                                     page,
                                                     await AccountsViewModel.ForAsync(_accountManager.Value,
                                                                                      _memberManager.Value,
                                                                                      _contentLocator.Value,
                                                                                      _lookups.Value,
                                                                                      _accountIdentityAccessor.Value.Get(),
                                                                                      query),
                                                     await SignInViewModel.ForAsync(_memberExternalLoginProviders.Value,
                                                                                    _contentLocator.Value.Single<SignInModalSettingsContent>(),
                                                                                    (await _memberManager.Value.GetCurrentPublishedMemberAsync()).As<MemberContent>(),
                                                                                    _memberManager.Value.IsLoggedIn()));

        return viewModel;
    }
}