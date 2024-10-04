using Flurl;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingUrlBuilder {
    string GenerateUrl(string path, Action<Url> addQueryParameters = null);
    
    IContentLocator ContentLocator { get; }
}