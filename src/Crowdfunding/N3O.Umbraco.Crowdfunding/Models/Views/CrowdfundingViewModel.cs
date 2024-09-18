using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public abstract class CrowdfundingViewModel : ICrowdfundingViewModel {
    private Func<SelectAccountViewModel> _getSelectAccount;
    private Func<SignInViewModel> _getSignIn;
    private SelectAccountViewModel _selectAccount;
    private SignInViewModel _signIn;

    public IContentLocator ContentLocator { get; private set; }
    public IFormatter Formatter { get; private set; }
    public ICrowdfundingPage Page { get; private set; }
    public MemberContent Member { get; private set; }
    public bool IsSignedIn { get; private set; }
    public AccountIdentity Account { get; private set; }
    public bool AccountSelected => Account.HasValue(x => x.Id);

    public string Link(Func<IContentLocator, string> getUrl) {
        return getUrl(ContentLocator);
    }

    public SelectAccountViewModel SelectAccount() {
        if (_selectAccount == null) {
            _selectAccount = _getSelectAccount();
        }

        return _selectAccount;
    }
    
    public SignInViewModel SignIn() {
        if (_signIn == null) {
            _signIn = _getSignIn();
        }

        return _signIn;
    }

    public static T For<T>(IContentLocator contentLocator,
                           IFormatter formatter,
                           ICrowdfundingPage page,
                           MemberContent member,
                           bool isSignedIn,
                           AccountIdentity account,
                           Func<SelectAccountViewModel> getSelectAccount,
                           Func<SignInViewModel> getSignIn)
        where T : CrowdfundingViewModel, new() {
        var viewModel = new T();
        
        viewModel.ContentLocator = contentLocator;
        viewModel.Formatter = formatter;
        viewModel.Page = page;
        viewModel.Member = member;
        viewModel.IsSignedIn = isSignedIn;
        viewModel.Account = account;
        viewModel._getSelectAccount = getSelectAccount;
        viewModel._getSignIn = getSignIn;

        return viewModel;
    }
}