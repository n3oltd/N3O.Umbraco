using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfundingViewModel {
    IContentLocator ContentLocator { get; }
    IFormatter Formatter { get; }
    ICrowdfundingPage Page { get; }
    MemberContent Member { get; }
    bool IsSignedIn { get; }
    AccountIdentity Account { get; }
    bool AccountSelected { get; }

    string Link(Func<IContentLocator, string> getUrl);
    SelectAccountViewModel SelectAccount();
    SignInViewModel SignIn();
}