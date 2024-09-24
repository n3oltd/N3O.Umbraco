using N3O.Umbraco.Crowdfunding.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingViewModelFactory {
    Task<T> CreateViewModelAsync<T>(ICrowdfundingPage page, IReadOnlyDictionary<string, string> query)
        where T : CrowdfundingViewModel, new();
}