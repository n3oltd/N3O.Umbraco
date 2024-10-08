using N3O.Umbraco.Content;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfundingViewModel {
    IContentLocator ContentLocator { get; }
    ICrowdfundingUrlBuilder UrlBuilder { get; }
    IFormatter Formatter { get; }
    ICrowdfundingPage Page { get; }
    AccountsViewModel Accounts { get; }
    SignInViewModel SignIn { get; }

    string Link(Func<ICrowdfundingUrlBuilder, string> getUrl);
    
}