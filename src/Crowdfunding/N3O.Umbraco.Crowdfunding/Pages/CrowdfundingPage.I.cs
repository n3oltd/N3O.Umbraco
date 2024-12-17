using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.OpenGraph;
using Smidge;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingPage {
    void AddAssets(ISmidgeRequire bundle);
    void AddOpenGraph(IOpenGraphBuilder builder, Uri requestUri, IReadOnlyDictionary<string, string> requestQuery);
    string GetPageTitle(Uri requestUri, IReadOnlyDictionary<string, string> requestQuery);
    bool IsMatch(Uri requestUri, IReadOnlyDictionary<string, string> requestQuery);
    Task<ICrowdfundingViewModel> GetViewModelAsync(Uri requestUri, IReadOnlyDictionary<string, string> requestQuery);
    
    PageAccess Access { get; }
    bool NoCache { get; }
    string ViewName { get; }
}