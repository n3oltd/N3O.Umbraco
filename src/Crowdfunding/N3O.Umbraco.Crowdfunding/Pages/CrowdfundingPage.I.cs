using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using Smidge;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingPage {
    void AddAssets(ISmidgeRequire bundle);
    bool IsMatch(Uri requestUri, IReadOnlyDictionary<string, string> requestQuery);
    Task<ICrowdfundingViewModel> GetViewModelAsync(Uri requestUri, IReadOnlyDictionary<string, string> requestQuery);
    
    PageAccess Access { get; }
    bool NoCache { get; }
    string ViewName { get; }
}