using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System;
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
    
    public async Task<T> CreateViewModelAsync<T>(ICrowdfundingPage page) where T : CrowdfundingViewModel, new() {
        SelectAccountViewModel GetSelectAccount() {
            return SelectAccountViewModel.ForAsync(_accountManager.Value,
                                                   _memberManager.Value,
                                                   _contentLocator.Value,
                                                   _lookups.Value)
                                         .GetAwaiter()
                                         .GetResult();
        }
        
        SignInViewModel GetSignIn() {
            var signInSettingsContent = _contentLocator.Value.Single<SignInModalSettingsContent>();
            
            return SignInViewModel.ForAsync(_memberExternalLoginProviders.Value, signInSettingsContent)
                                  .GetAwaiter()
                                  .GetResult();
        }

        var viewModel = CrowdfundingViewModel.For<T>(_contentLocator.Value,
                                                     _formatter.Value,
                                                     page,
                                                     (await _memberManager.Value.GetCurrentPublishedMemberAsync()).As<MemberContent>(),
                                                     _memberManager.Value.IsLoggedIn(),
                                                     _accountIdentityAccessor.Value.Get(),
                                                     GetSelectAccount,
                                                     GetSignIn);

        return viewModel;
    }
}