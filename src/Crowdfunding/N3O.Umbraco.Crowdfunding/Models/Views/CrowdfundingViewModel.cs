using N3O.Umbraco.Content;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public abstract class CrowdfundingViewModel : ICrowdfundingViewModel {
    public IContentLocator ContentLocator { get; private set; }
    public IFormatter Formatter { get; private set; }
    public ICrowdfundingPage Page { get; private set; }
    public AccountsViewModel Accounts { get; private set; }
    public SignInViewModel SignIn { get; private set; }

    public string Link(Func<IContentLocator, string> getUrl) {
        return getUrl(ContentLocator);
    }

    public static T For<T>(IContentLocator contentLocator,
                           IFormatter formatter,
                           ICrowdfundingPage page,
                           AccountsViewModel accounts,
                           SignInViewModel signIn)
        where T : CrowdfundingViewModel, new() {
        var viewModel = new T();
        
        viewModel.ContentLocator = contentLocator;
        viewModel.Formatter = formatter;
        viewModel.Page = page;
        viewModel.Accounts = accounts;
        viewModel.SignIn = signIn;

        return viewModel;
    }
}